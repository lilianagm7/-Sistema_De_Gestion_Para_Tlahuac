using System.ComponentModel.DataAnnotations;

namespace Sistema_Actividades_Tlahuac.Models.Enums
{
    public enum EstadoInscripcion
    {
        [Display(Name = "Pendiente")]
        Pendiente = 1,

        [Display(Name = "Confirmada")]
        Confirmada = 2,

        [Display(Name = "Cancelada")]
        Cancelada = 3,

        [Display(Name = "Espera")]
        ListaEspera = 4
    }
}
