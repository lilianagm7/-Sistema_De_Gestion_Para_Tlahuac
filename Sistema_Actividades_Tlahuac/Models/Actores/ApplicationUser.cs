using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Sistema_Actividades_Tlahuac.Models.Actores
{
    public class ApplicationUser:IdentityUser
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido paterno es obligatorio")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El apellido materno es obligatorio")]
        public string ApellidoMaterno { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
