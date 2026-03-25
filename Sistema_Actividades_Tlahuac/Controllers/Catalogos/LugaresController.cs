using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Microsoft.AspNetCore.Authorization;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Services.Catalogos;

namespace Sistema_Actividades_Tlahuac.Controllers.Catalogos
{
    [Authorize]
    public class LugaresController : Controller
    {
        private readonly LugarService _lugarService;

        public LugaresController(LugarService context)
        {
            _lugarService = context;
        }

        // GET: Lugares
        public async Task<IActionResult> Index(string? buscador, bool mostrarInactivos = false)
        {
            var lugares = await _lugarService.ObtenerTodos(buscador, mostrarInactivos);
            return View(lugares);
        }

        // GET: Lugares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lugares/Create
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Colonia,Seccion,Direccion,Latitud,Longitud")] Lugar lugar)
        {
            if (!ModelState.IsValid)
            {
                return View(lugar);
            }
            try
            {
                await _lugarService.Create(lugar);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View(lugar);
            }

        }

        // GET: Lugares/Edit/5
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lugar = await _lugarService.ObtenerPorId(id.Value);
            if (lugar == null)
            {
                return NotFound();
            }
            return View(lugar);
        }

        // POST: Lugares/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Colonia,Seccion,Direccion,Latitud,Longitud,Estado")] Lugar lugar)
        {
            if (id != lugar.Id) { return NotFound();}

            if (!ModelState.IsValid) { return View(lugar); }

            try
            {
                await _lugarService.Edit(lugar);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(lugar);
            }
            
        }


        // GET: Lugares/Delete/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lugar = await _lugarService.ObtenerPorId(id.Value);

                
            if (lugar == null)
            {
                return NotFound();
            }

            return View(lugar);
        }

        // POST: Lugares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _lugarService.Desactivar(id);
            return RedirectToAction("Index");
        }

    }
}
