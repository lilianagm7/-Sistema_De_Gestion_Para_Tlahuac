using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Inscripciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Actividades_Tlahuac.Models.Alumnos
{
    public class Alumno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string ApellidoMaterno { get; set; }

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {ApellidoPaterno} {ApellidoMaterno}";

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        // Responsable adulto
        [Required]
        public string UsuarioResponsableId { get; set; }

        [ForeignKey(nameof(UsuarioResponsableId))]
        [ValidateNever]
        public ApplicationUser UsuarioResponsable { get; set; }

        // Relación con catálogo de parentesco
        [Required(ErrorMessage = "El parentesco es obligatorio")]
        [Display(Name = "Parentesco")]
        public int ParentescoId { get; set; }

        [ForeignKey(nameof(ParentescoId))]
        [ValidateNever]
        public Parentesco Parentesco { get; set; }

        // Datos opcionales útiles
        [StringLength(20)]
        public string? Curp { get; set; }

        [StringLength(20)]
        public string? TelefonoContacto { get; set; }

        // Registro histórico
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //Relacion con la tabla usuarios
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

        // Inscripciones del alumno
        public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
    }
}