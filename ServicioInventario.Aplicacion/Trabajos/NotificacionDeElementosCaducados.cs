using ServicioInventario.Aplicacion.Notificador;
using System;

namespace ServicioInventario.Aplicacion.Trabajos
{
    /// <summary>
    /// Clase que define el trabajo de notificación de elementos caducados
    /// </summary>
    public class NotificacionDeElementosCaducados : IDisposable
    {
        private readonly IGestorInventario GestorInventario;
        private readonly INotificador Notificador;

        /// <summary>
        /// Crea una nueva instancia de NotificacionDeElementosCaducados con el GestorInventario y Notificador por defecto
        /// </summary>
        public NotificacionDeElementosCaducados()
        {
            GestorInventario = new GestorInventario();
            Notificador = new NotificadorTexto();
        }

        /// <summary>
        /// Crea una nueva instancia de NotificacionDeElementosCaducados
        /// </summary>
        /// <param name="gestorInventario">Implementación de <see cref="IGestorInventario"/></param>
        /// <param name="notificador">Implementación de <see cref="INotificador"/></param>
        public NotificacionDeElementosCaducados(IGestorInventario gestorInventario, INotificador notificador)
        {
            GestorInventario = gestorInventario ?? throw new ArgumentNullException(nameof(gestorInventario));
            Notificador = notificador ?? throw new ArgumentNullException(nameof(notificador));
        }

        /// <summary>
        /// Ejecuta el trabajo de notificación de elementos caducados
        /// </summary>
        public void Ejecutar()
        {
            var elementosCaducados = GestorInventario.ObtenerCaducadosSinNotificar();

            foreach (var elemento in elementosCaducados)
            {
                Notificador.Notificar("Se ha caduado el elemento de inventario:", elemento);

                GestorInventario.MarcarCaducidadNotificada(elemento.Id);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GestorInventario.Dispose();
        }
    }
}
