using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Eventos;
using Sistema_Actividades_Tlahuac.Models.ViewModels.Eventos;

namespace Sistema_Actividades_Tlahuac.Services.Eventos
{
    public class EventoService
    {
        private readonly ApplicationDbContext _context;

        public EventoService(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly IWebHostEnvironment _env;
        public EventoService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // PARA EL INDEX
        public async Task<List<EventoIndexViewModel>> ObtenerEventosParaIndex()
        {
            return await _context.Eventos
                .Include(e => e.Espacio)
                .Include(e => e.Categoria)
                .Where(e => e.Estado == EstadoEvento.Activo)
                .OrderBy(e => e.FechaInicio)
                .Select(e => new EventoIndexViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin,
                    EspacioNombre = e.Espacio.Nombre,
                    CategoriaNombre = e.Categoria.Nombre,
                    CuposDisponibles = e.CuposDisponibles,
                    ImagenUrl = e.ImagenUrl
                })
                .ToListAsync();
        }

        // DETALLES
        public async Task<EventoDetailsViewModel?> ObtenerDetalle(int id)
        {
            return await _context.Eventos
                .Include(e => e.Espacio)
                .Include(e => e.Categoria)
                .Where(e => e.Id == id)
                .Select(e => new EventoDetailsViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin,
                    EspacioNombre = e.Espacio.Nombre,
                    CapacidadEspacio = e.Espacio.Capacidad,
                    CategoriaNombre = e.Categoria.Nombre,
                    CapacidadMaxima = e.CapacidadMaxima,
                    CuposDisponibles = e.CuposDisponibles,
                    ImagenUrl = e.ImagenUrl,
                    Estado = e.Estado,
                    FechaCreacion = e.FechaCreacion,
                    FechaModificacion = e.FechaModificacion,
                    UsuarioModificacion = e.UsuarioModificacion
                })
                .FirstOrDefaultAsync();
        }


        public async Task<string?> GuardarImagen(IFormFile? imagen)
        {
            if (imagen == null)
                return null;

            //VALIDAR TAMAÑO (2MB)
            if (imagen.Length > 2 * 1024 * 1024)
                throw new Exception("La imagen no debe superar los 2MB");

            //VALIDAR EXTENSIÓN
            var extension = Path.GetExtension(imagen.FileName).ToLower();
            var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png" };

            if (!extensionesPermitidas.Contains(extension))
                throw new Exception("Formato de imagen no permitido");

            //VALIDAR MIME TYPE (extra seguridad)
            if (!imagen.ContentType.StartsWith("image/"))
                throw new Exception("El archivo no es una imagen válida");

            //CREAR CARPETA SI NO EXISTE
            var carpeta = Path.Combine(_env.WebRootPath, "imagenes/eventos");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            //NOMBRE ÚNICO
            var nombreArchivo = Guid.NewGuid().ToString() + extension;

            var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

            //GUARDAR
            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            //DEVOLVER URL RELATIVA
            return "/imagenes/eventos/" + nombreArchivo;
        }

        //CREAR EVENTO
        public async Task CrearEvento(EventoCreateViewModel model, string usuarioId)
        {
            //VALIDACIONES 
            //FECHAS
            if (model.FechaFin <= model.FechaInicio)
                throw new Exception("La fecha de fin debe ser mayor a la de inicio");
            if (model.FechaInicio < DateTime.Today)
                throw new Exception("No puedes crear eventos en fechas pasadas");
            //ESPACIOS
            var espacio = await _context.Espacios.FindAsync(model.EspacioId);
            if (espacio == null)
                throw new Exception("Espacio no válido");
            //CAPACIDAD
            if (model.CapacidadMaxima > espacio.Capacidad)
                throw new Exception("La capacidad excede el espacio");

            //VALIDAR CONFLICTO DE ESPACIO
            var conflicto = await _context.Eventos
                .AnyAsync(e =>
                    e.EspacioId == model.EspacioId &&
                    e.Estado == EstadoEvento.Activo &&
                    (
                        model.FechaInicio < e.FechaFin &&
                        model.FechaFin > e.FechaInicio
                    )
                );

            if (conflicto)
                throw new Exception("El espacio ya está ocupado en ese rango de fechas");

            var imagenUrl = await GuardarImagen(model.Imagen);

            //CREACION
            var evento = new Evento
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,
                CapacidadMaxima = model.CapacidadMaxima,
                //CuposDisponibles = model.CapacidadMaxima,
                EspacioId = model.EspacioId,
                CategoriaId = model.CategoriaId,
                Estado = EstadoEvento.Activo,
                FechaCreacion = DateTime.UtcNow,
                UsuarioCreacion = usuarioId,
                ImagenUrl = imagenUrl
            };

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        //
        public async Task CargarCombosCrear(EventoCreateViewModel model)
        {
            model.Espacios = await _context.Espacios
                .Where(e => e.Estado == EstadoRegistro.Activo)
                .Select(e => new EspaciosDetalleVM
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Capacidad = e.Capacidad
                })
                .ToListAsync();

            model.Categorias = await _context.Categorias
                .Where(c => c.Estado == EstadoRegistro.Activo)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                })
                .ToListAsync();
        }

        //EDITAR EVENTOS

        public async Task<EventoEditViewModel?> ObtenerParaEditar(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null) return null;

            return new EventoEditViewModel
            {
                Id = evento.Id,
                Nombre = evento.Nombre,
                Descripcion = evento.Descripcion,
                FechaInicio = evento.FechaInicio,
                FechaFin = evento.FechaFin,
                EspacioId = evento.EspacioId,
                CategoriaId = evento.CategoriaId,
                CapacidadMaxima = evento.CapacidadMaxima,
                Estado = evento.Estado,
                ImagenActualUrl = evento.ImagenUrl,
                EventoYaInicio = evento.FechaInicio <= DateTime.Now
            };
        }

        public async Task EditarEvento(EventoEditViewModel model, string usuarioId)
        {
            var evento = await _context.Eventos.FindAsync(model.Id);

            if (evento == null)
                throw new Exception("Evento no encontrado");

            var eventoYaInicio = evento.FechaInicio <= DateTime.Now;

            // Si ya inició, bloquea campos sensibles
            if (eventoYaInicio)
            {
                if (model.FechaInicio != evento.FechaInicio ||
                    model.FechaFin != evento.FechaFin ||
                    model.CapacidadMaxima != evento.CapacidadMaxima ||
                    model.EspacioId != evento.EspacioId)
                {
                    throw new Exception("No puedes modificar fechas, capacidad ni espacio de un evento que ya inició.");
                }
            }
            else
            {
                // Validaciones normales solo si el evento aún no inicia
                if (model.FechaFin <= model.FechaInicio)
                    throw new Exception("La fecha de fin debe ser mayor a la de inicio");

                if (model.FechaInicio <= DateTime.Now)
                    throw new Exception("No puedes usar fechas pasadas");

                var espacio = await _context.Espacios.FindAsync(model.EspacioId);
                if (espacio == null)
                    throw new Exception("Espacio no válido");

                if (model.CapacidadMaxima > espacio.Capacidad)
                    throw new Exception("La capacidad excede el espacio");

                var conflicto = await _context.Eventos.AnyAsync(e =>
                    e.Id != evento.Id &&
                    e.EspacioId == model.EspacioId &&
                    e.Estado == EstadoEvento.Activo &&
                    model.FechaInicio < e.FechaFin &&
                    model.FechaFin > e.FechaInicio);

                if (conflicto)
                    throw new Exception("El espacio ya está ocupado en ese rango de fechas");
            }

            // Imagen
            if (model.Imagen != null)
            {
                if (!string.IsNullOrEmpty(evento.ImagenUrl))
                {
                    var rutaAnterior = Path.Combine(_env.WebRootPath, evento.ImagenUrl.TrimStart('/'));

                    if (File.Exists(rutaAnterior))
                        File.Delete(rutaAnterior);
                }

                var nuevaRuta = await GuardarImagen(model.Imagen);
                evento.ImagenUrl = nuevaRuta;
            }

            // Campos permitidos siempre
            evento.Nombre = model.Nombre;
            evento.Descripcion = model.Descripcion;
            evento.CategoriaId = model.CategoriaId;
            evento.Estado = model.Estado;

            // Solo si aún no inicia
            if (!eventoYaInicio)
            {
                evento.FechaInicio = model.FechaInicio;
                evento.FechaFin = model.FechaFin;
                evento.EspacioId = model.EspacioId;
                evento.CapacidadMaxima = model.CapacidadMaxima;
            }

            evento.UsuarioModificacion = usuarioId;
            evento.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task CargarCombosEditar(EventoEditViewModel model)
        {
            model.Espacios = await _context.Espacios
                .Where(e => e.Estado == EstadoRegistro.Activo)
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Nombre
                }).ToListAsync();

            model.Categorias = await _context.Categorias
                .Where(c => c.Estado == EstadoRegistro.Activo)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                }).ToListAsync();
        }

        //DESACTIVACION DE EVENTOS
        public async Task<EventoDeleteViewModel?> ObtenerParaEliminar(int id)
        {
            return await _context.Eventos
                .Where(e => e.Id == id)
                .Select(e => new EventoDeleteViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    ImagenUrl = e.ImagenUrl,
                    LugarNombre = e.Espacio.Lugar.Nombre,
                    EspacioNombre = e.Espacio.Nombre,
                    CategoriaNombre = e.Categoria.Nombre,
                    CapacidadMaxima = e.CapacidadMaxima,
                    CapacidadEspacio = e.Espacio.Capacidad,
                    CuposDisponibles = e.CuposDisponibles,
                    FechaInicio = e.FechaInicio,
                    FechaFin = e.FechaFin
                })
                .FirstOrDefaultAsync();
        }

        public async Task EliminarEvento(int id, string usuarioId)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
                throw new Exception("Evento no encontrado");

            evento.Estado = EstadoEvento.Cancelado;
            evento.FechaModificacion = DateTime.UtcNow;
            evento.UsuarioModificacion = usuarioId;

            await _context.SaveChangesAsync();
        }
    }
}