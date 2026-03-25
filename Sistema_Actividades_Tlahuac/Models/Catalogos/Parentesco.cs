using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Actividades_Tlahuac.Models.Catalogos
{
    public class Parentesco
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "¿Qué es del alumno?")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessage = "Máximo 250 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        //Registro historico
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //Relacion con la tabla usuarios
        public string? UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioCreacion")]
        [ValidateNever]
        public ApplicationUser Usuario { get; set; }
        //Modificacion
        public DateTime? FechaModificacion { get; set; } = DateTime.Now;
        public string? UsuarioModificacion { get; set; }
        [ForeignKey("UsuarioModificacion")]
        [ValidateNever]
        public ApplicationUser Us_Modifica { get; set; }

        /*
        // Relación donde parentesco puede tener muchos alumnos
        public ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();
        */
    }
}
