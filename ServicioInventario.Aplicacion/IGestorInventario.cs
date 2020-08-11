using ServicioInventario.Modelos;
using System.Collections.Generic;

namespace ServicioInventario.Aplicacion
{
    /// <summary>
    /// Interfaz con las operaciones necesaria para la gestión de inventario
    /// </summary>
    public interface IGestorInventario
    {
        /// <summary>
        /// Inserta un nuevo elemento de inventario
        /// </summary>
        /// <param name="elementoInventario">El <see cref="ElementoInventario"/> a ser insertado</param>
        void Insertar(ElementoInventario elementoInventario);

        /// <inheritdoc/>
        void Dispose();

        /// <summary>
        /// Marca un elemento de inventario indicando que su caducidad ha sido notificada
        /// </summary>
        /// <param name="Id">Identificado del elemento de inventario</param>
        void MarcarCaducidadNotificada(int Id);

        /// <summary>
        /// Obtiene los elementos de inventario caducados pendiente de notificación
        /// </summary>
        /// <returns>Enumerador con los <see cref="ElementoInventario"/> buscados</returns>
        IEnumerable<ElementoInventario> ObtenerCaducadosSinNotificar();

        /// <summary>
        /// Devuelve el elemento de inventario con menor fecha de caducidad a partir de su nombre y lo elimina del inventario
        /// </summary>
        /// <param name="nombre">Nombre del elemento de inventario a ser obtenido</param>
        /// <returns>El <see cref="ElementoInventario"/> buscado</returns>
        ElementoInventario SacarPorNombre(string nombre);
    }
}