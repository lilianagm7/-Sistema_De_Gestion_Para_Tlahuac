using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Services.Catalogos;

namespace Sistema_Actividades_Tlahuac.Controllers.Catalogos
{
    [Authorize]
    public class ParentescosController : Controller
    {
        private readonly ParentescoService _parentescoService ;

        public ParentescosController(ParentescoService parentescoService)
        {
            _parentescoService = parentescoService;
        }

        // GET: Parentescos
        //Listado en orden por nombre
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Index(string? buscador, bool mostrarInactivos = false)
        {
            var parentescos = await _parentescoService.ObtenerTodas(buscador, mostrarInactivos);
            return View(parentescos);
        }


        // GET: Parentescos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parentescos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion")] Parentesco parentesco)
        {
            //Validar misma palabra con o sin acentos
            if (ModelState.IsValid)
            {
                // Validamos duplicados usando el servicio
                if (await _parentescoService.ExisteNombre(parentesco.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
                    return View(parentesco);
                }

                await _parentescoService.Crear(parentesco);
                return RedirectToAction(nameof(Index));
            }
            return View(parentesco);
        }


        // GET: Parentescos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var parentesco = await _parentescoService.ObtenerPorId(id.Value);
            if (parentesco == null) return NotFound();
            return View(parentesco);

        }

        // POST: Parentescos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Estado")] Parentesco parentesco)
        {
            if (id != parentesco.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Validamos duplicados excluyendo el mismo registro
                if (await _parentescoService.ExisteNombre(parentesco.Nombre, parentesco.Id))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
                    return View(parentesco);
                }

                await _parentescoService.Editar(parentesco);
                return RedirectToAction(nameof(Index));
            }
            return View(parentesco);
        }

        // GET: Parentescos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var parentesco = await _parentescoService.ObtenerPorId(id.Value);
            if (parentesco == null)
                return NotFound();

            return View(parentesco);
        }

        // POST: Parentescos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            await _parentescoService.Desactivar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
