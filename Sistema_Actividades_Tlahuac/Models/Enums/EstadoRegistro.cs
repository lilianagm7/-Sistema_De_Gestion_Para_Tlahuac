using System.ComponentModel.DataAnnotations;

namespace Sistema_Actividades_Tlahuac.Models.Enums
{
    public enum EstadoRegistro
    {
        [Display(Name = "Activo")]
        Activo = 1,
        [Display(Name = "Desactivado")]
        Inactivo = 2
    }
}
