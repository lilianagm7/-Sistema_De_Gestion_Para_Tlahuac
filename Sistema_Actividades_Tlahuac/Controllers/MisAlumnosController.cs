using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Alumnos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.ViewModels.Alumnos;
using Sistema_Actividades_Tlahuac.Services.Actores;
using Sistema_Actividades_Tlahuac.Services.Alumnos;
using System.Security.Claims;

namespace Sistema_Actividades_Tlahuac.Controllers
{
    [Authorize(Roles = "Usuario")]
    public class MisAlumnosController : Controller
    {
        private readonly IAlumnoService _alumnoService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MisAlumnosController(
            IAlumnoService alumnoService,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _alumnoService = alumnoService;
            _context = context;
            _userManager = userManager;
        }

        // GET: MisAlumnos
        public async Task<IActionResult> Index(string? busqueda)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var alumnos = await _alumnoService.ObtenerPorResponsableAsync(userId, busqueda);
            ViewBag.Busqueda = busqueda;

            return View(alumnos);
        }

        // GET: MisAlumnos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
                return BadRequest();

            var alumno = await _alumnoService.ObtenerPorIdAsync(id);

            if (alumno == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (alumno.UsuarioResponsableId != userId)
                return Forbid();

            return View(alumno);
        }

        // GET: MisAlumnos/Create
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var usuario = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuario == null)
                return Challenge();

            var parentescos = await _context.Parentescos
                .AsNoTracking()
                .Where(p => p.Estado == EstadoRegistro.Activo)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            var vm = new AlumnoCreateViewModel
            {
                EsPropio = true,
                Nombre = usuario.Nombre ?? string.Empty,
                ApellidoPaterno = usuario.ApellidoPaterno ?? string.Empty,
                ApellidoMaterno = usuario.ApellidoMaterno ?? string.Empty,
                Parentescos = parentescos.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                })
            };

            ViewBag.ParentescoPropioId = await ObtenerParentescoPropioIdAsync();
            return View(vm);
        }


        // POST: MisAlumnos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlumnoCreateViewModel vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var usuario = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuario == null)
                return Challenge();

            var parentescoPropioId = await ObtenerParentescoPropioIdAsync();

            if (vm.EsPropio)
            {
                if (parentescoPropioId == 0)
                {
                    ModelState.AddModelError(string.Empty, "No existe el parentesco PROPIO en el catálogo.");
                }
            }
            else
            {
                if (vm.ParentescoId == null || vm.ParentescoId <= 0)
                    ModelState.AddModelError(nameof(vm.ParentescoId), "Debes seleccionar un parentesco.");

                if (vm.ParentescoId == parentescoPropioId)
                    ModelState.AddModelError(nameof(vm.ParentescoId), "No puedes seleccionar PROPIO para un dependiente.");
            }

            if (!ModelState.IsValid)
            {
                var parentescos = await _context.Parentescos
                    .AsNoTracking()
                    .Where(p => p.Estado == EstadoRegistro.Activo)
                    .OrderBy(p => p.Nombre)
                    .ToListAsync();

                vm.Parentescos = parentescos.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                });

                ViewBag.ParentescoPropioId = parentescoPropioId;
                return View(vm);
            }

            var alumno = new Alumno
            {
                UsuarioResponsableId = userId,
                Estado = vm.Estado,
                FechaNacimiento = vm.FechaNacimiento,
                TelefonoContacto = string.IsNullOrWhiteSpace(vm.TelefonoContacto) ? null : vm.TelefonoContacto.Trim(),
                Curp = string.IsNullOrWhiteSpace(vm.Curp) ? null : vm.Curp.Trim().ToUpperInvariant()
            };

            if (vm.EsPropio)
            {
                alumno.ParentescoId = parentescoPropioId;
                alumno.Nombre = usuario.Nombre ?? string.Empty;
                alumno.ApellidoPaterno = usuario.ApellidoPaterno ?? string.Empty;
                alumno.ApellidoMaterno = usuario.ApellidoMaterno ?? string.Empty;
            }
            else
            {
                alumno.ParentescoId = vm.ParentescoId!.Value;
                alumno.Nombre = vm.Nombre.Trim().ToUpperInvariant();
                alumno.ApellidoPaterno = vm.ApellidoPaterno.Trim().ToUpperInvariant();
                alumno.ApellidoMaterno = vm.ApellidoMaterno.Trim().ToUpperInvariant();
            }

            var resultado = await _alumnoService.CrearAsync(alumno, esUsuarioExterno: !vm.EsPropio);

            if (!resultado.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultado.Mensaje);

                var parentescos = await _context.Parentescos
                    .AsNoTracking()
                    .Where(p => p.Estado == EstadoRegistro.Activo)
                    .OrderBy(p => p.Nombre)
                    .ToListAsync();

                vm.Parentescos = parentescos.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nombre
                });

                ViewBag.ParentescoPropioId = parentescoPropioId;
                return View(vm);
            }

            TempData["Success"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        // GET: MisAlumnos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
                return BadRequest();

            var alumno = await _alumnoService.ObtenerPorIdAsync(id);

            if (alumno == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (alumno.UsuarioResponsableId != userId)
                return Forbid();

            return View(alumno);
        }

        // POST: MisAlumnos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Alumno alumno)
        {
            if (id != alumno.Id)
                return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
                return Challenge();

            var existente = await _alumnoService.ObtenerPorIdAsync(id);

            if (existente == null)
                return NotFound();

            if (existente.UsuarioResponsableId != userId)
                return Forbid();

            var parentescoPropioId = await ObtenerParentescoPropioIdAsync();
            var esPropio = parentescoPropioId > 0 && existente.ParentescoId == parentescoPropioId;

            alumno.UsuarioResponsableId = userId;

            if (esPropio)
            {
                var usuario = await _userManager.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (usuario == null)
                    return Challenge();

                ModelState.Remove(nameof(Alumno.Nombre));
                ModelState.Remove(nameof(Alumno.ApellidoPaterno));
                ModelState.Remove(nameof(Alumno.ApellidoMaterno));
                ModelState.Remove(nameof(Alumno.ParentescoId));

                alumno.ParentescoId = parentescoPropioId;
                alumno.Nombre = usuario.Nombre ?? string.Empty;
                alumno.ApellidoPaterno = usuario.ApellidoPaterno ?? string.Empty;
                alumno.ApellidoMaterno = usuario.ApellidoMaterno ?? string.Empty;

                var resultado = await _alumnoService.ActualizarAsync(alumno, esUsuarioExterno: false);

                if (!resultado.Exitoso)
                {
                    ModelState.AddModelError(string.Empty, resultado.Mensaje);
                    return View(alumno);
                }

                TempData["Success"] = resultado.Mensaje;
                return RedirectToAction(nameof(Index));
            }

            // Dependiente: conserva su parentesco original
            alumno.ParentescoId = existente.ParentescoId;

            var resultadoDependiente = await _alumnoService.ActualizarAsync(alumno, esUsuarioExterno: true);

            if (!resultadoDependiente.Exitoso)
            {
                ModelState.AddModelError(string.Empty, resultadoDependiente.Mensaje);
                return View(alumno);
            }

            TempData["Success"] = resultadoDependiente.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        // POST: MisAlumnos/Desactivar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Desactivar(int id)
        {
            if (id <= 0)
                return BadRequest();

            var alumno = await _alumnoService.ObtenerPorIdAsync(id);

            if (alumno == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (alumno.UsuarioResponsableId != userId)
                return Forbid();

            var resultado = await _alumnoService.DesactivarAsync(id);

            if (!resultado.Exitoso)
            {
                TempData["Error"] = resultado.Mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        // POST: MisAlumnos/Activar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activar(int id)
        {
            if (id <= 0)
                return BadRequest();

            var alumno = await _alumnoService.ObtenerPorIdAsync(id);

            if (alumno == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (alumno.UsuarioResponsableId != userId)
                return Forbid();

            var resultado = await _alumnoService.ActivarAsync(id);

            if (!resultado.Exitoso)
            {
                TempData["Error"] = resultado.Mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = resultado.Mensaje;
            return RedirectToAction(nameof(Index));
        }

        private async Task<int> ObtenerParentescoPropioIdAsync()
        {
            return await _context.Parentescos
                .Where(p => p.Estado == EstadoRegistro.Activo && p.Nombre == "PROPIO")
                .Select(p => p.Id)
                .FirstOrDefaultAsync();
        }

        private async Task CargarParentescosAsync(int? selectedParentescoId = null)
        {
            var parentescos = await _context.Parentescos
                .AsNoTracking()
                .Where(p => p.Estado == EstadoRegistro.Activo && p.Nombre != "PROPIO")
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            ViewBag.Parentescos = new SelectList(parentescos, "Id", "Nombre", selectedParentescoId);
        }
    }
}