using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Actores;

namespace Sistema_Actividades_Tlahuac.Models.Talleres
{
    public class Taller : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del taller es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nombre del taller")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessage = "Máximo 250 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public DateTime FechaFin { get; set; }

        // Relación con Espacio (lugar del evento)
        [Required(ErrorMessage = "El espacio es obligatorio")]
        [Display(Name = "Espacio")]
        public int EspacioId { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        [Display(Name = "Capacidad máxima")]
        public int CapacidadMaxima { get; set; }

        public string? ImagenUrl { get; set; }
        // Control interno 
        public int CuposDisponibles { get; private set; }
        //public int CuposDisponibles => CapacidadMaxima - Inscripciones.Count;

        // quien gestiona
        public string? AdministradorId { get; set; }
        public ApplicationUser Administrador { get; set; }
        public string? CoordinadorId { get; set; }
        public ApplicationUser Coordinador { get; set; }


        //Registro historico
        public EstadoEvento Estado { get; set; } = EstadoEvento.Activo;
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public string? UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioCreacion")]
        [ValidateNever]
        public ApplicationUser Usuario { get; set; }
        //Modificacion
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        [ForeignKey("UsuarioModificacion")]
        [ValidateNever]
        public ApplicationUser Us_Modifica { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        // Lógica interna
        public void InicializarCupos()
        {
            CuposDisponibles = CapacidadMaxima;
        }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (FechaFin <= FechaInicio)
            {
                yield return new ValidationResult(
                    "La fecha de fin debe ser mayor a la de inicio",
                    new[] { nameof(FechaFin) });
            }

            if (FechaInicio.Date < DateTime.Today)
            {
                yield return new ValidationResult(
                    "No se pueden crear eventos en fechas pasadas",
                    new[] { nameof(FechaInicio) });
            }
        }
    }
}
