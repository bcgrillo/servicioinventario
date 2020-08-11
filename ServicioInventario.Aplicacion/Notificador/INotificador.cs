namespace ServicioInventario.Aplicacion.Notificador
{
    /// <summary>
    /// Interfaz para componentes de notificación
    /// </summary>
    public interface INotificador
    {
        /// <summary>
        /// Genera una nueva notificación
        /// </summary>
        /// <param name="tipo">Texto que indica el motivo de la notificación</param>
        /// <param name="informacion">Objeto que genera la notificación</param>
        void Notificar(string tipo, object informacion);
    }
}
