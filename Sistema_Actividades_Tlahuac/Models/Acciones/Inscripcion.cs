using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Alumnos;
using Sistema_Actividades_Tlahuac.Models.Eventos;
using Sistema_Actividades_Tlahuac.Models.Talleres;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Actividades_Tlahuac.Models.Inscripciones
{
    public class Inscripcion : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public int AlumnoId { get; set; }

        [ForeignKey(nameof(AlumnoId))]
        [ValidateNever]
        public Alumno Alumno { get; set; }

        // Puede ser evento o taller, pero no ambos
        public int? EventoId { get; set; }

        [ForeignKey(nameof(EventoId))]
        [ValidateNever]
        public Evento? Evento { get; set; }

        public int? TallerId { get; set; }

        [ForeignKey(nameof(TallerId))]
        [ValidateNever]
        public Taller? Taller { get; set; }

        [Display(Name = "Fecha de inscripción")]
        public DateTime FechaInscripcion { get; set; } = DateTime.UtcNow;


        //Registro historico
        [Display(Name = "Estado")]
        public EstadoInscripcion Estado { get; set; } = EstadoInscripcion.Pendiente;

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public string? UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioCreacion")]
        [ValidateNever]
        public ApplicationUser? UsuarioCrea { get; set; }

        //Modificacion
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        [ForeignKey("UsuarioModificacion")]
        [ValidateNever]
        public ApplicationUser Us_Modifica { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool tieneEvento = EventoId.HasValue;
            bool tieneTaller = TallerId.HasValue;

            if (tieneEvento == tieneTaller)
            {
                yield return new ValidationResult(
                    "La inscripción debe pertenecer a un evento o a un taller, pero no a ambos.",
                    new[] { nameof(EventoId), nameof(TallerId) });
            }
        }
    }
}