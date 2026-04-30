using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*nueva actualizacion*/
namespace Sistema_Actividades_Tlahuac.Models.Actores
{
    public class Instructor
    {
        public int Id { get; set; }

        // Relación con Identity
        [Required(ErrorMessage = "El usuario es obligatorio")]
        [Display(Name = "Usuario")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual ApplicationUser Usuario { get; set; }

        [Required(ErrorMessage = "El RFC es obligatorio")]
        [StringLength(13)]
        public string RFC { get; set; }

        [StringLength(250)]
        [Display(Name = "Direccción")]
        public string? Direccion { get; set; }
        
        [StringLength(250)]
        [Display(Name = "Fotografia")]
        public string? FotoUrl { get; set; }

        [Display(Name = "Grado escolar")]
        public GradoAcademico NivelAcademico { get; set; }

        [Display(Name = "Descripción del grado academico")]
        [StringLength(150)]
        public string? DescripcionGrado { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        public string Especialidad { get; set; }

        [Display(Name = "Tipo de contrato")]
        public TipoContrato TipoContrato { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }

        
        [Display(Name = "Fecha de ingreso")]
        [Required(ErrorMessage = "La fecha de ingreo es obligatoria")]
        public DateTime? FechaIngreso { get; set; }

        [Display(Name = "Fecha de fin de contrato")]
        public DateTime? FechaFinContrato { get; set; }

        [Phone]
        public string? Telefono { get; set; }

        [EmailAddress]
        [Display(Name = "Correo alterno")]
        public string? EmailContacto { get; set; }

        //Registro historico
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;
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
    }
}