using System.ComponentModel.DataAnnotations;

namespace Sistema_Actividades_Tlahuac.Models.ViewModels.Eventos
{
    public class EventoDeleteViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Lugar")]
        public string LugarNombre { get; set; }

        [Display(Name = "Espacio")]
        public string EspacioNombre { get; set; }

        [Display(Name = "Capacidad")]
        public int CapacidadEspacio { get; set; }

        [Display(Name = "Categoria")]
        public string CategoriaNombre { get; set; }

        [Display(Name = "Capacidad maxima")]
        public int CapacidadMaxima { get; set; }

        [Display(Name = "Cupos disponibles")]
        public int CuposDisponibles { get; set; }

        [Display(Name = "Imagen")]
        public string? ImagenUrl { get; set; }
    }
}
