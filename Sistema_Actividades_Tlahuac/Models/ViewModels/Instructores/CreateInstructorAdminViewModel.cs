using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Actividades_Tlahuac.Models.ViewModels.Instructores
{
    public class CreateInstructorAdminViewModel
    {
        //Datos de acceso
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electronico")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        //Datos personales
        [Required]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido paterno")]
        public string ApellidoPaterno { get; set; }
        [Display(Name = "Apellido materno")]
        public string ApellidoMaterno { get; set; }

        //Datos instructor
        public string RFC { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        public IFormFile Foto { get; set; }
        [Display(Name = "nivel academico")]
        public GradoAcademico NivelAcademico { get; set; }

        [Display(Name = "Descripción del grado academico")]
        public string DescripcionGrado { get; set; }


        public string Especialidad { get; set; }

        [Display(Name = "Tipo de contrato")]
        public string TipoContrato { get; set; }

        public decimal Salario { get; set; }

        [Required]
        [Display(Name = "Fecha de ingreso")]
        public DateTime FechaIngreso { get; set; }
        [Display(Name = "Fecha fin de contrato")]
        public DateTime? FechaFinContrato { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string EmailContacto { get; set; }

    }
}
