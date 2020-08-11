namespace ServicioInventario.WebAPI.Trabajos
{
    /// <summary>
    /// Clase estática para la llamada al trabajo de notificación de elementos caducados
    /// </summary>
    public static class NotificacionDeElementosCaducados
    {
        /// <summary>
        /// Llama a la ejecuón de notificación de elementos caducados
        /// </summary>
        public static void Ejecutar()
        {
            using (var trabajo = new Aplicacion.Trabajos.NotificacionDeElementosCaducados())
            {
                trabajo.Ejecutar();
            }
        }
    }
}