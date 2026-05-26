using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Alumnos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Services.Actores;
using Sistema_Actividades_Tlahuac.Services.Alumnos;

namespace Sistema_Actividades_Tlahuac.Areas.Admin.Controllers.Actores
{
    [Area("Admin")]

    public class AlumnosController : Controller
    {
        private readonly IAlumnoService _alumnoService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AlumnosController(
            IAlumnoService alumnoService,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _alumnoService = alumnoService;
            _context = context;
            _userManager = userManager;
        }

        // GET: Alumnos
        public async Task<IActionResult> Index(string? busqueda)
        {
            var alumnos = await _alumnoService.ObtenerTodosAsync(busqueda);
            ViewBag.Busqueda = busqueda;
            return View(alumnos);
        }

        // GET: Alumnos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return BadRequest();

            var alumno = await _alumnoService.ObtenerPorIdAsync(id);

            if (alumno == null)
                return NotFound();

            return View(alumno);
        }

        // GET: Alumnos/Create
        public async Task<IActionResult> Create()
        {
            await CargarCombosAsync();
            ViewBag.EsEdicion = false;
            ViewBag.EsUsuarioExterno = true;

            return View(new Alumno
            {
                Estado = EstadoRegistro.Activo
            });
        }

        // POST: Alumnos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Alumno alumno, bool esUsuarioExterno)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombosAsync();
                ViewBag.EsEdicion = false;
                ViewBag.EsUsuarioExterno = esUsuarioExterno;
                return View(alumno);
            }

            var resultado = await _alumnoService.CrearAsync(alumno, esUsuarioExterno);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                await CargarCombosAsync();
                ViewBag.EsEdicion = false;
                ViewBag.EsUsuarioExterno = esUsuarioExterno;
                return View(alumno);
            }

            TempData["Success"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        // GET: Alumnos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var alumno = await _alumnoService.ObtenerPorIdAsync(id);

            if (alumno == null)
                return NotFound();

            await CargarCombosAsync();
            ViewBag.EsEdicion = true;
            ViewBag.EsUsuarioExterno = true;

            return View(alumno);
        }

        // POST: Alumnos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Alumno alumno, bool esUsuarioExterno)
        {
            if (id != alumno.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await CargarCombosAsync();
                ViewBag.EsEdicion = true;
                ViewBag.EsUsuarioExterno = esUsuarioExterno;
                return View(alumno);
            }

            var resultado = await _alumnoService.ActualizarAsync(alumno, esUsuarioExterno);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);
                await CargarCombosAsync();
                ViewBag.EsEdicion = true;
                ViewBag.EsUsuarioExterno = esUsuarioExterno;
                return View(alumno);
            }

            TempData["Success"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        // POST: Alumnos/Desactivar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            if (id <= 0)
                return BadRequest();

            var resultado = await _alumnoService.DesactivarAsync(id);

            if (!resultado.Exitoso)
            {
                TempData["Error"] = resultado.Mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarCombosAsync()
        {
            var parentescos = await _context.Parentescos
                .AsNoTracking()
                .Where(p => p.Estado == EstadoRegistro.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            ViewBag.Parentescos = new SelectList(parentescos, "Id", "Nombre");

            var usuarios = await _userManager.Users
                .AsNoTracking()
                .OrderBy(u => u.UserName)
                .ToListAsync();

            ViewBag.UsuariosResponsables = new SelectList(usuarios, "Id", "UserName");
        }
    }
}