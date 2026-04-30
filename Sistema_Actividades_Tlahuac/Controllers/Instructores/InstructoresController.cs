using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Services.Instructores;

namespace Sistema_Actividades_Tlahuac.Controllers.Instructores
{
    public class InstructoresController : Controller
    {
        private readonly InstructorService _instructorService;

        public InstructoresController(InstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        // GET: Instructores
        public async Task<IActionResult> Index(string? buscador, bool mostrarInactivos = false)
        {
            var instructores = await _instructorService.ObtenerTodos(buscador, mostrarInactivos);
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
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = await _instructorService.ObtenerUsuariosDisponibles();
                return View(instructor);
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _instructorService.Crear(instructor, userId);

                return RedirectToAction(nameof(Index));

                foreach (var campo in ModelState)
                {
                    foreach (var error in campo.Value.Errors)
                    {
                        Console.WriteLine($"{campo.Key}: {error.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
               
                return View(instructor);
            }

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
        public async Task<IActionResult> Edit(int id, Instructor instructor)
        {
            if (id != instructor.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(instructor);

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _instructorService.Editar(instructor, userId);

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
