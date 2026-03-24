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
    public class EspaciosController : Controller
    {
        private readonly EspacioService _espacioService;

        public EspaciosController(EspacioService espacioService)
        {
            _espacioService = espacioService;
        }

        // GET: Espacios
        public async Task<IActionResult> Index(string? buscador, bool mostrarInactivos = false)
        {
            //Organizados por nombre y por activos o no
            var espacios = await _espacioService.ObtenerTodas(buscador, mostrarInactivos);
            return View(espacios);
        }

        // GET: Espacios/Create
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Espacios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Nombre,Capacidad,Estado")] Espacio espacio)
        {
            //Validamos
            if (!ModelState.IsValid)

                return View(espacio);

            try
            {
                await _espacioService.Create(espacio);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(espacio);
            }
        }

        // GET: Espacios/Edit/5
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null) return NotFound();

            var espacio = await _espacioService.ObtenerPorId(id.Value);
            if (espacio == null) return NotFound();

            return View(espacio);

        }

        // POST: Espacios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador, Coordinador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Capacidad,Estado")] Espacio espacio)
        {
            if (id != espacio.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(espacio);

            try
            {
                await _espacioService.Edit(espacio);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(espacio);
            }
        }

        // GET: Espacios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var espacio = await _espacioService.ObtenerPorId(id.Value);
            if (espacio == null) return NotFound();

            return View(espacio);
        }

        // POST: Espacios/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            await _espacioService.Desactivar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
