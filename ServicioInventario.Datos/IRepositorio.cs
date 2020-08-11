using ServicioInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServicioInventario.Datos
{
    public interface IRepositorio<T> : IDisposable where T : class, IEntidad
    {
        void Crear(T entidad);

        IEnumerable<T> Consultar(Func<IQueryable<T>, IQueryable<T>> consulta);

        void Actualizar(int Id, Action<T> actualizacion);

        void Eliminar(int Id);
    }
}
