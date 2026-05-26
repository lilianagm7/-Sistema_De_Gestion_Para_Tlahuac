using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Actividades_Tlahuac.Models.Enums;

namespace Sistema_Actividades_Tlahuac.Models.ViewModels.Alumnos
{
    public class AlumnoCreateViewModel
    {
        public bool EsPropio { get; set; } = true;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }

        public string? TelefonoContacto { get; set; }

        public string? Curp { get; set; }

        [Required(ErrorMessage = "Debes seleccionar un parentesco")]
        public int? ParentescoId { get; set; }

        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

        [ValidateNever]
        public IEnumerable<SelectListItem>? Parentescos { get; set; }
    }
}