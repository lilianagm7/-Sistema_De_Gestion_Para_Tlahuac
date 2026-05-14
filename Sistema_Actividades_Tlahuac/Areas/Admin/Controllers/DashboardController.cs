using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Enums;

namespace Sistema_Actividades_Tlahuac.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DashboardController
        public ActionResult Index()
        {
            // Totales
            ViewBag.TotalEventos = _context.Eventos.Count();
            ViewBag.TotalTalleres = _context.Talleres.Count();
            ViewBag.TotalInstructores = _context.Instructores.Count();
            ViewBag.TotalCategorias = _context.Categorias.Count();

            // Eventos activos/inactivos
            ViewBag.EventosActivos = _context.Eventos.Count(e => e.Estado == EstadoEvento.Activo);

            ViewBag.EventosInactivos = _context.Eventos.Count(e => e.Estado == EstadoEvento.Activo);

            // Categorías (para gráfica)
            var categorias = _context.Categorias
                .Select(c => new
                {
                    c.Nombre,
                    Total = _context.Eventos.Count(e => e.CategoriaId == c.Id)
                })
                .ToList();

            ViewBag.CategoriasLabels = System.Text.Json.JsonSerializer.Serialize(
                categorias.Select(c => c.Nombre)
            );

            ViewBag.CategoriasData = System.Text.Json.JsonSerializer.Serialize(
                categorias.Select(c => c.Total)
            );

            return View();
        }
    }
}
