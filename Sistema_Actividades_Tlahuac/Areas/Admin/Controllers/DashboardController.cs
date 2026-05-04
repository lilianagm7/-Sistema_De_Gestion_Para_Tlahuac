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

        // GET: DashboardController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DashboardController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DashboardController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashboardController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashboardController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DashboardController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
