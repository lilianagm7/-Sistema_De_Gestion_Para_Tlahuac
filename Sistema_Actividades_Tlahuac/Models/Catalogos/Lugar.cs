using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Sistema_Actividades_Tlahuac.Models.Catalogos
{
    public class Lugar
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La colonia es obligatoria")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string? Colonia { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Sección")]
        public string? Seccion { get; set; }

        [StringLength(250, ErrorMessage = "Máximo 250 caracteres")]
        [Display(Name = "Dirección")]
        public string? Direccion { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "Latitud inválida")]
        public double? Latitud { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitud inválida")]
        public double? Longitud { get; set; }

    //Registro historico
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //Relacion con la tabla usuarios
        public string? UsuarioCreacion { get; set; }
        [ValidateNever]
        public ApplicationUser Usuario { get; set; }


        //Relacion con tabla espacios
        public ICollection<Espacio> Espacios { get; set; } = new List<Espacio>();
    }
}
