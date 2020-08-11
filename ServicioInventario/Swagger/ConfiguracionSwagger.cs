using Microsoft.Web.Http.Routing;
using Swashbuckle.Application;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Routing;

namespace ServicioInventario.WebAPI.Swagger
{
    /// <summary>
    /// Clase para la configuración de Swagger
    /// </summary>
    public static class ConfiguracionSwagger
    {
        /// <summary>
        /// Registra la configuración de Swagger
        /// </summary>
        /// <param name="config">Elemento <see cref="HttpConfiguration"/> donde realizar la configuración</param>
        /// <param name="XmlCommentsFilePath">Ruta del fichero XML generado durante la construcción</param>
        public static void Registrar(HttpConfiguration config, string XmlCommentsFilePath)
        {
            var constraintResolver = new DefaultInlineConstraintResolver() { ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) } };

            config.AddApiVersioning(o => o.ReportApiVersions = true);
            config.MapHttpAttributeRoutes(constraintResolver);

            //Habilitar Swagger
            var apiExplorer = config.AddVersionedApiExplorer(c => c.GroupNameFormat = "'v'VVVV");

            config.EnableSwagger("{apiVersion}/swagger", c =>
            {
                c.MultipleApiVersions(
                    (apiDescription, version) => apiDescription.GetGroupName() == version,
                    info =>
                    {
                        foreach (var group in apiExplorer.ApiDescriptions)
                        {
                            info.Version(group.Name, $"Servicio Inventario API {group.ApiVersion}")
                                .Contact(cb => cb.Name("Bruno Grillo").Email("bcgrillo@yahoo.com"))
                                .Description("WebAPI para la gestión de inventario.");
                        }
                    });

                //Filtro de operaciones para valores predeterminados
                c.OperationFilter<ValoresPorDefecto>();

                //Incluye comentarios XML
                c.IncludeXmlComments(XmlCommentsFilePath);

                //Raiz para las peticiones
                c.RootUrl(req => req.RequestUri.GetLeftPart(UriPartial.Authority) + req.GetRequestContext().VirtualPathRoot.TrimEnd('/'));
            })
                .EnableSwaggerUi(swagger => swagger.EnableDiscoveryUrlSelector());
        }
    }
}