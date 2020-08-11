using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioInventario.Modelos
{
    /// <summary>
    /// Clase que representa un elemento de inventario
    /// </summary>
    public class ElementoInventario : IEntidad, ICloneable
    {
        /// <inheritdoc/>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del elemento
        /// </summary>
        [Required, MinLength(1), MaxLength(900), Index(IsUnique = false)]
        public string Nombre { get; set; }

        /// <summary>
        /// Fecha de caducidad (en UTC)
        /// </summary>
        [Required]
        public DateTime FechaDeCaducidadUTC { get; set; }

        /// <summary>
        /// Tipo del elemento
        /// </summary>
        [Required, MinLength(1), MaxLength(900), Index(IsUnique = false)]
        public string Tipo { get; set; }

        /// <summary>
        /// Indica si la caducidad ya ha sido notificada
        /// </summary>
        public bool CaducidadNotificada { get; set; }

        /// <inheritdoc/>
        public object Clone()
        {
            return new ElementoInventario()
            {
                Id = this.Id,
                Nombre = this.Nombre,
                FechaDeCaducidadUTC = this.FechaDeCaducidadUTC,
                Tipo = this.Tipo,
                CaducidadNotificada = this.CaducidadNotificada
            };
        }
    }
}
