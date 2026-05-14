using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Actividades_Tlahuac.Models.Eventos
{
    public class Evento : IValidatableObject
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.UtcNow;
        public DateTime FechaFin { get; set; } = DateTime.UtcNow;
        public int CapacidadMaxima { get; set; }

        //Relacion con espacio
        public int EspacioId { get; set; }
        public Espacio Espacio { get; set; }

        //Relacion con categoria
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

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

        //relacion con inscripcion
        //public ICollection<Inscripcion> Inscripciones { get; set; }

        //Validacion de fechas
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

