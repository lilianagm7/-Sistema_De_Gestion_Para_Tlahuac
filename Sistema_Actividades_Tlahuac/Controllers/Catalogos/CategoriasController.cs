using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Services;
using Microsoft.AspNetCore.Authorization;

namespace Sistema_Actividades_Tlahuac.Controllers.Catalogos
{
    [Authorize]
    public class CategoriasController : Controller
    {
        //accesos denegados 
        public IActionResult AccessDenied()
        {
            return View();
        }

        //llamada al servicio
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: Categorias
        //Listado en orden por nombre
        public async Task<IActionResult> Index(bool mostrarInactivos = false)
        {
            //return View(await _context.Categorias.ToListAsync());
            var categorias = await _categoriaService.ObtenerTodas(mostrarInactivos);
            return View(categorias);
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var categorias = await _categoriaService.ObtenerTodas(true);
            var categoria = categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        [Authorize(Roles = "Administrador")]
        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Estado")] Categoria categoria)
        {
            //Validar misma palabra con o sin acentos
            if (ModelState.IsValid)
            {
                // Validamos duplicados usando el servicio
                if (await _categoriaService.ExisteNombre(categoria.Nombre))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
                    return View(categoria);
                }

                await _categoriaService.Crear(categoria);
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
        [Authorize(Roles = "Administrador, Coordinador")]
        public async Task<IActionResult> Edit(int? id)
        {
            var categorias = await _categoriaService.ObtenerTodas(true);
            var categoria = categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Estado")] Categoria categoria)
        {
            if (id != categoria.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                // Validamos duplicados excluyendo el mismo registro
                if (await _categoriaService.ExisteNombre(categoria.Nombre, categoria.Id))
                {
                    ModelState.AddModelError("Nombre", "Ya existe una categoría con ese nombre.");
                    return View(categoria);
                }

                await _categoriaService.Editar(categoria);
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }


        [Authorize(Roles = "Administrador")]
        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var categorias = await _categoriaService.ObtenerTodas(true);
            var categoria = categorias.FirstOrDefault(c => c.Id == id);

            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaService.Desactivar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
