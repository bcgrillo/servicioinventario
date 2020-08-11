using Swashbuckle.Swagger;
using System.Linq;
using System.Web.Http.Description;

namespace ServicioInventario.WebAPI.Swagger
{
    /// <summary>
    /// Clase que implementa un filtro de operaciones de Swagger para la configuración de valores por defecto.
    /// </summary>
    public class ValoresPorDefecto : IOperationFilter
    {
        /// <inheritdoc/>
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.parameters)
            {
                var description = apiDescription.ParameterDescriptions
                                                .FirstOrDefault(p => p.Name == parameter.name);

                if (parameter.description == null)
                {
                    parameter.description = description?.Documentation;
                }

                if (parameter.@default == null)
                {
                    parameter.@default = description?.ParameterDescriptor.DefaultValue;
                }
            }
        }
    }
}