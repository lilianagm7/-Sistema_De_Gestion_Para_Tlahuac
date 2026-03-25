using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Actividades_Tlahuac.Models.Eventos
{
    public class Evento : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del evento es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        [Display(Name = "Nombre del evento")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessage = "Máximo 250 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        // Relación con Espacio (el lugar real del evento)
        [Required(ErrorMessage = "El espacio es obligatorio")]
        [Display(Name = "Espacio")]
        public int EspacioId { get; set; }

        [ForeignKey("EspacioId")]
        [ValidateNever]
        public Espacio Espacio { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        [Display(Name = "Capacidad máxima")]
        public int CapacidadMaxima { get; set; }

        [Display(Name = "Imagen")]
        public string? ImagenUrl { get; set; }

        // Control interno
        public int CuposDisponibles { get; private set; }

        public EstadoEvento Estado { get; set; } = EstadoEvento.Planeado;

        // quien gestiona
        public string? AdministradorId { get; set; }
        public string? CoordinadorId { get; set; }

        //relación opcional con talleres
        //public ICollection<Taller> Talleres { get; set; } = new List<Taller>();

        //Registro historico
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public string? UsuarioCreacion { get; set; }

        [ForeignKey("UsuarioCreacion")]
        [ValidateNever]
        public ApplicationUser? Usuario { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string? UsuarioModificacion { get; set; }

        [ForeignKey("UsuarioModificacion")]
        [ValidateNever]
        public ApplicationUser? Us_Modifica { get; set; }

        // Control de concurrencia para no sobrescribir cambios simultáneos
        [Timestamp]
        public byte[] RowVersion { get; set; }

        //Validacion de cupos
        public void InicializarCupos()
        {
            CuposDisponibles = CapacidadMaxima;
        }

        public void ReducirCupo()
        {
            if (CuposDisponibles <= 0)
                throw new InvalidOperationException("No hay cupos disponibles");

            CuposDisponibles--;
        }

        public void LiberarCupo()
        {
            if (CuposDisponibles < CapacidadMaxima)
                CuposDisponibles++;
        }

        //Validacion de fecha inicial
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
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