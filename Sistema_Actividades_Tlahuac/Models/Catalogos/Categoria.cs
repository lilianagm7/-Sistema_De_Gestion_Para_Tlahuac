using System.ComponentModel.DataAnnotations;
using Sistema_Actividades_Tlahuac.Models.Enums;

namespace Sistema_Actividades_Tlahuac.Models.Catalogos
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Máximo 250 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
        //Concexion con el enum de estados
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;

        // Relación donde categoria puede tener muchos eventos
        //public ICollection<Evento>? Eventos { get; set; } = new List<Evento>();

    }
}
