using Microsoft.AspNetCore.Mvc;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.ViewModels.Eventos;
using Sistema_Actividades_Tlahuac.Services.Eventos;

namespace Sistema_Actividades_Tlahuac.Controllers
{

    public class EventosController : Controller
    {
        private readonly EventoService _eventoService;

        public EventosController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            var eventos = await _eventoService.ObtenerEventosParaIndex();
            return View(eventos);
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var evento = await _eventoService.ObtenerDetalle(id.Value);

            if (evento == null)
                return NotFound();

            return View(evento);
        }
    }
}