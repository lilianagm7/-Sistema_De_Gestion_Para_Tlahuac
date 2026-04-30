using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sistema_Actividades_Tlahuac.Models.ViewModels.Eventos
{
    public class EventoCreateViewModel
    {
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

        // Relación con Espacio (el lugar real del evento)
        [Required(ErrorMessage = "El espacio es obligatorio")]
        [Display(Name = "Espacio")]
        public int EspacioId { get; set; }

        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacidad debe ser mayor a 0")]
        [Display(Name = "Capacidad máxima")]
        public int CapacidadMaxima { get; set; }

        public int CuposDisponibles { get; set; }


        [Required(ErrorMessage = "La categoría es obligatoria")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile? Imagen { get; set; }

        public List<EspaciosDetalleVM> Espacios { get; set; } = new();
        public List<SelectListItem> Categorias { get; set; } = new();
    }
}
