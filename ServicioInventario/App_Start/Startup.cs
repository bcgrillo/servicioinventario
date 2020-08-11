using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Microsoft.Owin;
using Owin;
using ServicioInventario.WebAPI.Swagger;
using ServicioInventario.WebAPI.Trabajos;
using System;
using System.Reflection;
using System.Web.Http;

[assembly: OwinStartup(typeof(ServicioInventario.WebAPI.App_Start.Startup))]
namespace ServicioInventario.WebAPI.App_Start
{
    /// <summary>
    /// Clase que define el arranque de la aplicación web
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configura la aplicación
        /// </summary>
        /// <param name="app">Constructor de aplicaciones de Owin</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            //Swagger
            ConfiguracionSwagger.Registrar(config, XmlCommentsFilePath(typeof(Startup)));

            //Detalles de errores
#if DEBUG
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
#else
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.LocalOnly;
#endif

            //Configuración Json
            config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            config.Formatters.JsonFormatter.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;

            //WebApi
            app.UseWebApi(config);

            //Hangfire (para la gestión de trabajos asíncronos)
            Hangfire.GlobalConfiguration.Configuration.UseMemoryStorage();
            app.UseHangfireServer();
            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate(() => NotificacionDeElementosCaducados.Ejecutar(), Cron.Minutely);
        }

        private static string XmlCommentsFilePath(Type startupType)
        {
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath;
            var fileName = startupType.GetTypeInfo().Assembly.GetName().Name + ".xml";
            return System.IO.Path.Combine(basePath, fileName);
        }
    }
}