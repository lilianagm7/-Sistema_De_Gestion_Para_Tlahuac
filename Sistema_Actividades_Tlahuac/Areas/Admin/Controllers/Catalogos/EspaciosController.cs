using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Services.Catalogos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Sistema_Actividades_Tlahuac.Areas.Admin.Controllers.Catalogos
{
    [Area("Admin")]
    [Authorize]
    public class EspaciosController : Controller
    {
        private readonly EspacioService _espacioService;
        private readonly LugarService _lugarService;

        public EspaciosController(EspacioService espacioService, LugarService lugarService)
        {
            _espacioService = espacioService;
            _lugarService = lugarService;
        }

        // GET: Espacios
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Index(string? buscador, bool mostrarInactivos = false)
        {
            // Organizados por nombre y filtrados por estado
            var espacios = await _espacioService.ObtenerTodas(buscador, mostrarInactivos);
            return View(espacios);
        }

        // GET: Espacios/Create
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            // Cargamos los lugares activos para el dropdown
            var lugares = await _lugarService.ObtenerTodos(null, false);

            ViewData["LugarId"] = new SelectList(
                lugares.Select(l => new
                {
                    l.Id,
                    NombreCompleto = l.Nombre + " - " + l.Colonia
                }),
                "Id",
                "NombreCompleto"
            );

            return View();
        }

        // POST: Espacios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Nombre,Capacidad,LugarId")] Espacio espacio)
        {
            // Validamos el modelo
            if (!ModelState.IsValid)
            {
                // Recargamos dropdown si falla validación
                var lugares = await _lugarService.ObtenerTodos(null, false);
                

                ViewData["LugarId"] = new SelectList(
                    lugares.Select(l => new
                    {
                        l.Id,
                        NombreCompleto = l.Nombre + " - " + l.Colonia
                    }),
                    "Id",
                    "NombreCompleto",
                    espacio.LugarId
                );

                return View(espacio);
            }

            try
            {
                // Guardamos usando el servicio
                await _espacioService.Create(espacio);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Mostramos error del servicio
                ModelState.AddModelError("", ex.Message);
                var lugares = await _lugarService.ObtenerTodos(null, false);

                ViewData["LugarId"] = new SelectList(
                    lugares.Select(l => new
                    {
                        l.Id,
                        NombreCompleto = l.Nombre + " - " + l.Colonia
                    }),
                    "Id",
                    "NombreCompleto",
                    espacio.LugarId
                );

                return View(espacio);
            }
        }

        // GET: Espacios/Edit/5
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var espacio = await _espacioService.ObtenerPorId(id.Value);
            if (espacio == null) return NotFound();

            // Cargar lugares para edición
            var lugares = await _lugarService.ObtenerTodos(null, false);

            ViewData["LugarId"] = new SelectList(
                lugares.Select(l => new
                {
                    l.Id,
                    NombreCompleto = l.Nombre + " - " + l.Colonia
                }),
                "Id",
                "NombreCompleto",
                espacio.LugarId
            );

            return View(espacio);
        }

        // POST: Espacios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Capacidad,Estado,LugarId")] Espacio espacio)
        {
            if (id != espacio.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                var lugares = await _lugarService.ObtenerTodos(null, false);

                ViewData["LugarId"] = new SelectList(
                    lugares.Select(l => new
                    {
                        l.Id,
                        NombreCompleto = l.Nombre + " - " + l.Colonia
                    }),
                    "Id",
                    "NombreCompleto",
                    espacio.LugarId
                );

                return View(espacio);
            }

            try
            {
                await _espacioService.Edit(espacio);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Mostramos error del servicio
                ModelState.AddModelError("", ex.Message);

                // IMPORTANTE: recargar dropdown también aquí
                var lugares = await _lugarService.ObtenerTodos(null, false);

                ViewData["LugarId"] = new SelectList(
                    lugares.Select(l => new
                    {
                        l.Id,
                        NombreCompleto = l.Nombre + " - " + l.Colonia
                    }),
                    "Id",
                    "NombreCompleto",
                    espacio.LugarId
                );

                return View(espacio);
            }
        }

        // GET: Espacios/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var espacio = await _espacioService.ObtenerPorId(id.Value);
            if (espacio == null) return NotFound();

            return View(espacio);
        }

        // POST: Espacios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            // No eliminamos, solo desactivamos
            await _espacioService.Desactivar(id);

            return RedirectToAction(nameof(Index));
        }
    }
}