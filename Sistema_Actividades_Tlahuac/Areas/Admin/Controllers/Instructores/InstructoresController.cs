using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Services.Eventos;
using Sistema_Actividades_Tlahuac.Services.Instructores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sistema_Actividades_Tlahuac.Areas.Admin.Controllers.Instructores
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador")]
    public class InstructoresController : Controller
    {
        private readonly InstructorService _instructorService;

        public InstructoresController(InstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        // GET: Instructores
        // GET: Instructores
        public async Task<IActionResult> Index(string? buscador, string? buscadorRfc, bool mostrarInactivos = false)
        {
            var instructores = await _instructorService.ObtenerTodos(
                buscador,
                buscadorRfc,
                mostrarInactivos);

            return View(instructores);
        }




        // GET: Instructores/Create
        public async Task<IActionResult> Create()
        {
            ViewData["UserId"] = await _instructorService.ObtenerUsuariosDisponibles();
            return View();
        }

        // POST: Instructores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor, IFormFile? FotoArchivo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = await _instructorService.ObtenerUsuariosDisponibles();
                return View(instructor);
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _instructorService.Crear(instructor, userId, FotoArchivo);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["UserId"] = await _instructorService.ObtenerUsuariosDisponibles();
                return View(instructor);
            }
        }

        // GET: Instructores/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var instructor = await _instructorService.ObtenerDetalles(id);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        // GET: Instructores/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var instructor = await _instructorService.ObtenerPorId(id);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        // POST: Instructores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor, IFormFile? FotoArchivo)
        {
            if (id != instructor.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(instructor);

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _instructorService.Editar(instructor, userId, FotoArchivo);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(instructor);
            }
        }

        // GET: Instructores/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var instructor = await _instructorService.ObtenerPorId(id);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        // POST: Instructores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _instructorService.Desactivar(id, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var instructor = await _instructorService.ObtenerPorId(id);
                return View("Delete", instructor);
            }
        }
    }
}
