using Newtonsoft.Json;
using System;
using System.IO;

namespace ServicioInventario.Aplicacion.Notificador
{
    /// <summary>
    /// Clase que implementa el interfaz <see cref="INotificador"/> para la escritura de notificaciones en fichero de texto
    /// </summary>
    public class NotificadorTexto : INotificador
    {
        ///<inheritdoc/>
        public void Notificar(string mensaje, object objeto)
        {
            using (var sw = File.AppendText(Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, "output.txt")))
            {
                sw.WriteLine(DateTime.UtcNow.ToString("s"));
                sw.WriteLine(mensaje);
                sw.WriteLine(JsonConvert.SerializeObject(objeto));
            }
        }
    }
}
