namespace ServicioInventario.Modelos
{
    /// <summary>
    /// Interfaz para entidades que tienen un identificador entero 'Id'
    /// </summary>
    public interface IEntidad
    {
        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        int Id { get; set; }
    }
}
