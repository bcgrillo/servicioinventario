using ServicioInventario.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ServicioInventario.Datos
{
    public class RepositorioXML<T> : IRepositorio<T> where T : class, IEntidad, ICloneable, new()
    {
        private readonly List<T> Lista;
        private readonly string RutaArchivo;

        public RepositorioXML()
        {
            RutaArchivo = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, $"datos_{new T().GetType().Name}.xml");

            if (File.Exists(RutaArchivo))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

                FileStream fs = new FileStream(RutaArchivo, FileMode.Open);
                XmlReader reader = XmlReader.Create(fs);

                Lista = (List<T>)serializer.Deserialize(reader);

                fs.Close();
                reader.Close();
            }
            else
            {
                Lista = new List<T>();
            }
        }

        public void Crear(T entidad)
        {
            if (entidad == null) throw new ArgumentNullException(nameof(entidad));

            var entidadAux = (T)entidad.Clone();

            if (entidadAux.Id == 0) entidadAux.Id = Lista.Any() ? Lista.Max(e => e.Id) + 1 : 1;

            Lista.Add(entidadAux);
        }

        public IEnumerable<T> Consultar(Func<IQueryable<T>, IQueryable<T>> consulta)
        {
            return consulta(Lista.AsQueryable()).Select(e => (T)e.Clone()).ToList();
        }

        public void Actualizar(int Id, Action<T> actualizacion)
        {
            if (actualizacion == null) throw new ArgumentNullException(nameof(actualizacion));

            var elementos = Lista.Where(e => e.Id == Id);

            if (!elementos.Any()) throw new KeyNotFoundException();

            foreach (var entidad in elementos)
            {
                actualizacion(entidad);
            }
        }

        public void Eliminar(int Id)
        {
            Lista.RemoveAll(e => e.Id == Id);
        }

        public void Dispose()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            FileStream fs = new FileStream(RutaArchivo, FileMode.Create);
            TextWriter writer = new StreamWriter(fs, new UTF8Encoding());

            serializer.Serialize(writer, Lista);

            writer.Close();
            fs.Close();
        }
    }
}
