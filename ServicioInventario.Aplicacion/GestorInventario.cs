using ServicioInventario.Aplicacion.Notificador;
using ServicioInventario.Datos;
using ServicioInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicioInventario.Aplicacion
{
    /// <summary>
    /// Clase para la gestión de inventario
    /// </summary>
    public class GestorInventario : IDisposable, IGestorInventario
    {
        private readonly IRepositorio<ElementoInventario> Repositorio;
        private readonly INotificador Notificador;

        /// <summary>
        /// Crea una nueva instancia del gestor de inventarios con el repositorio y notificador por defecto
        /// </summary>
        public GestorInventario()
        {
            Repositorio = new RepositorioXML<ElementoInventario>();
            Notificador = new NotificadorTexto();
        }

        /// <summary>
        /// Crea una nueva instancia del gestor de inventarios
        /// </summary>
        /// <param name="repositorio">Implementación de <see cref="IRepositorio{ElementoInventario}"/></param>
        /// <param name="notificador">Implementación de <see cref="INotificador"/></param>
        public GestorInventario(IRepositorio<ElementoInventario> repositorio, INotificador notificador)
        {
            Repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            Notificador = notificador ?? throw new ArgumentNullException(nameof(notificador));
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        public void Insertar(ElementoInventario elementoInventario)
        {
            if (elementoInventario == null) throw new ArgumentNullException(nameof(elementoInventario));

            Repositorio.Crear(elementoInventario);
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="KeyNotFoundException"/>
        public ElementoInventario SacarPorNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) throw new ArgumentNullException(nameof(nombre));

            var elemento = Repositorio.Consultar(q => q.Where(e => e.Nombre == nombre).OrderBy(e => e.FechaDeCaducidadUTC).Take(1)).SingleOrDefault();

            if (elemento == null) throw new KeyNotFoundException();

            Repositorio.Eliminar(elemento.Id);

            Notificador.Notificar("Se ha sacado el elemento de inventario.", elemento);

            return elemento;
        }

        /// <inheritdoc/>
        public IEnumerable<ElementoInventario> ObtenerCaducadosSinNotificar()
        {
            return Repositorio.Consultar(q => q.Where(e => e.FechaDeCaducidadUTC < DateTime.UtcNow && !e.CaducidadNotificada));
        }

        /// <inheritdoc/>
        /// <exception cref="KeyNotFoundException"/>
        public void MarcarCaducidadNotificada(int Id)
        {
            Repositorio.Actualizar(Id, e => { e.CaducidadNotificada = true; });
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Repositorio.Dispose();
        }
    }
}
