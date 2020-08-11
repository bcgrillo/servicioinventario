using Microsoft.Web.Http;
using ServicioInventario.Aplicacion;
using ServicioInventario.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace ServicioInventario.WebAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de inventario
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("v{api-version:apiVersion}/inventario")]
    public class InventarioController : ApiController
    {
        /// <summary>
        /// Inserta un nuevo elemento de inventario
        /// </summary>
        /// <param name="elementoInventario"><see cref="ElementoInventario"/> que será insertado</param>
        /// <response code="201">El elemento ha sido insertado correctamente</response>
        [HttpPost]
        [Route]
        [ResponseType(typeof(void))]
        public IHttpActionResult Post([FromBody] ElementoInventario elementoInventario)
        {
            try
            {
                if (!ModelState.IsValid) throw new InvalidOperationException(string.Join(",", ModelState.SelectMany(s => s.Value.Errors.Select(e => e.ErrorMessage))));

                using (var gestor = new GestorInventario())
                {
                    gestor.Insertar(elementoInventario);
                }

                return CreatedAtRoute<ElementoInventario>(nameof(Get), new { nombre = elementoInventario.Nombre }, null);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.ToString());
            }
        }

        /// <summary>
        /// Obteniene el elemento de inventario con menor fecha de caducidad a partir de su nombre y lo elimina del inventario
        /// </summary>
        /// <param name="nombre">Nombre del elemento de inventario a ser obtenido</param>
        /// <returns>El <see cref="ElementoInventario"/> buscado</returns>
        /// <response code="200">El elemento ha sido devuelto correctamente</response>
        /// <response code="404">El elemento no ha sido encontrado</response>
        [HttpGet]
        [Route("{nombre}", Name = nameof(Get))]
        [ResponseType(typeof(ElementoInventario))]
        public IHttpActionResult Get(string nombre)
        {
            try
            {
                using (var gestor = new GestorInventario())
                {
                    return Ok(gestor.SacarPorNombre(nombre));
                }
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.ToString());
            }
        }
    }
}
