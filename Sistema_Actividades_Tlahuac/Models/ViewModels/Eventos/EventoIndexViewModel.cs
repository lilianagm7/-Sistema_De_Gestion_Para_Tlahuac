namespace Sistema_Actividades_Tlahuac.Models.ViewModels.Eventos
{
    public class EventoIndexViewModel
    {
        public int Id { get; set; }
        public string? ImagenUrl { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string EspacioNombre { get; set; }
        public string CategoriaNombre { get; set; }
        public int CuposDisponibles { get; set; }
    }
}
