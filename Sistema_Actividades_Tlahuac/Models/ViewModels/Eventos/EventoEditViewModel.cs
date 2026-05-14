using Microsoft.AspNetCore.Mvc.Rendering;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Actividades_Tlahuac.Models.ViewModels.Eventos
{
    public class EventoEditViewModel
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
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El espacio es obligatorio")]
        [Display(Name = "Espacio")]
        public int EspacioId { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        [Display(Name = "Capacidad máxima")]
        public int CapacidadMaxima { get; set; }
        public bool EventoYaInicio { get; set; }

        [Display(Name = "Estado")]
        public EstadoEvento Estado { get; set; }

        [Display(Name = "Imagen actual")]
        public string? ImagenActualUrl { get; set; }

        [Display(Name = "Nueva imagen")]
        public IFormFile? Imagen { get; set; }

        public List<SelectListItem> Espacios { get; set; } = new();
        public List<SelectListItem> Categorias { get; set; } = new();


    }
}
