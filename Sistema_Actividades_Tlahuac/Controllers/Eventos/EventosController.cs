using Microsoft.AspNetCore.Mvc;
using Sistema_Actividades_Tlahuac.Models.ViewModels.Eventos;
using Sistema_Actividades_Tlahuac.Services.Eventos;
using System.Security.Claims;

namespace Sistema_Actividades_Tlahuac.Controllers.Eventos
{
    public class EventosController : Controller
    {
        private readonly EventoService _eventoService;

        public EventosController(EventoService eventoService)
        {
            _eventoService = eventoService;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            var eventos = await _eventoService.ObtenerEventosParaIndex();
            return View(eventos);
        }

        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var evento = await _eventoService.ObtenerDetalle(id.Value);

            if (evento == null)
                return NotFound();

            return View(evento);
        }

        // GET: Eventos/Create
        public async Task<IActionResult> Create()
        {
            var model = new EventoCreateViewModel();
            await _eventoService.CargarCombosCrear(model);
            return View(model);
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventoCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var campo in ModelState)
                {
                    foreach (var error in campo.Value.Errors)
                    {
                        Console.WriteLine($"{campo.Key}: {error.ErrorMessage}");
                    }
                }
                await _eventoService.CargarCombosCrear(vm);
                return View(vm);
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _eventoService.CrearEvento(vm, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await _eventoService.CargarCombosCrear(vm);
                return View(vm);
            }
        }

        //GET: Eventos//Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var vm = await _eventoService.ObtenerParaEditar(id.Value);

            if (vm == null)
                return NotFound();

            await _eventoService.CargarCombosEditar(vm);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventoEditViewModel vm)
        {
            if (id != vm.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                await _eventoService.CargarCombosEditar(vm);
                return View(vm);
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _eventoService.EditarEvento(vm, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                await _eventoService.CargarCombosEditar(vm);
                return View(vm);
            }
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var evento = await _eventoService.ObtenerParaEliminar(id.Value);
            if (evento == null)
                return NotFound();
            return View(evento);
        }

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                await _eventoService.EliminarEvento(id, userId);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                var vm = await _eventoService.ObtenerParaEliminar(id);

                return View("Delete", vm);
            }
        }
    }
}