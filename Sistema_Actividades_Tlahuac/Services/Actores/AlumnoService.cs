using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Alumnos;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Inscripciones;
using Sistema_Actividades_Tlahuac.Services.Actores;

namespace Sistema_Actividades_Tlahuac.Services.Alumnos
{
    public class AlumnoService : IAlumnoService
    {
        private readonly ApplicationDbContext _context;

        public AlumnoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alumno>> ObtenerTodosAsync(string? busqueda = null)
        {
            var query = _context.Alumnos
                .Include(a => a.UsuarioResponsable)
                .Include(a => a.Parentesco)
                .AsNoTracking()
                .AsQueryable();

            query = AplicarBusqueda(query, busqueda);

            return await query
                .OrderBy(a => a.Nombre)
                .ThenBy(a => a.ApellidoPaterno)
                .ThenBy(a => a.ApellidoMaterno)
                .ToListAsync();
        }

        public async Task<List<Alumno>> ObtenerPorResponsableAsync(string usuarioResponsableId, string? busqueda = null)
        {
            if (string.IsNullOrWhiteSpace(usuarioResponsableId))
                return new List<Alumno>();

            var query = _context.Alumnos
                .Include(a => a.UsuarioResponsable)
                .Include(a => a.Parentesco)
                .AsNoTracking()
                .Where(a => a.UsuarioResponsableId == usuarioResponsableId)
                .AsQueryable();

            query = AplicarBusqueda(query, busqueda);

            return await query
                .OrderBy(a => a.Nombre)
                .ThenBy(a => a.ApellidoPaterno)
                .ThenBy(a => a.ApellidoMaterno)
                .ToListAsync();
        }

        public async Task<Alumno?> ObtenerPorIdAsync(int id)
        {
            if (id <= 0)
                return null;

            return await _context.Alumnos
                .Include(a => a.UsuarioResponsable)
                .Include(a => a.Parentesco)
                .Include(a => a.Inscripciones)
                    .ThenInclude(i => i.Evento)
                .Include(a => a.Inscripciones)
                    .ThenInclude(i => i.Taller)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(bool Exitoso, string Mensaje)> CrearAsync(Alumno alumno, bool esUsuarioExterno)
        {
            if (alumno == null)
                return (false, "El alumno no puede ser nulo.");

            var validacion = await ValidarAlumnoAsync(alumno, esEdicion: false, esUsuarioExterno);
            if (!validacion.Exitoso)
                return validacion;

            if (esUsuarioExterno)
            {
                alumno.Nombre = NormalizarTexto(alumno.Nombre);
                alumno.ApellidoPaterno = NormalizarTexto(alumno.ApellidoPaterno);
                alumno.ApellidoMaterno = NormalizarTexto(alumno.ApellidoMaterno);
            }
            else
            {
                // Aquí debes tomar los datos del usuario responsable
                var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == alumno.UsuarioResponsableId);
                if (usuario == null)
                    return (false, "El usuario responsable no existe.");

                alumno.Nombre = NormalizarTexto(usuario.Nombre);
                alumno.ApellidoPaterno = NormalizarTexto(usuario.ApellidoPaterno);
                alumno.ApellidoMaterno = NormalizarTexto(usuario.ApellidoMaterno);
            }

            _context.Alumnos.Add(alumno);
            await _context.SaveChangesAsync();

            return (true, "Alumno registrado correctamente.");
        }

        public async Task<(bool Exitoso, string Mensaje)> ActualizarAsync(Alumno alumno, bool esUsuarioExterno)
        {
            if (alumno == null)
                return (false, "El alumno no puede ser nulo.");

            var existente = await _context.Alumnos
                .Include(a => a.Inscripciones)
                .FirstOrDefaultAsync(a => a.Id == alumno.Id);

            if (existente == null)
                return (false, "El alumno no fue encontrado.");

            var validacion = await ValidarAlumnoAsync(alumno, esEdicion: true, esUsuarioExterno);
            if (!validacion.Exitoso)
                return validacion;

            existente.FechaNacimiento = alumno.FechaNacimiento;
            existente.UsuarioResponsableId = alumno.UsuarioResponsableId;
            existente.ParentescoId = alumno.ParentescoId;
            existente.Curp = string.IsNullOrWhiteSpace(alumno.Curp) ? null : alumno.Curp.Trim().ToUpperInvariant();
            existente.TelefonoContacto = string.IsNullOrWhiteSpace(alumno.TelefonoContacto) ? null : alumno.TelefonoContacto.Trim();
            existente.Estado = alumno.Estado;

            if (esUsuarioExterno)
            {
                existente.Nombre = NormalizarTexto(alumno.Nombre);
                existente.ApellidoPaterno = NormalizarTexto(alumno.ApellidoPaterno);
                existente.ApellidoMaterno = NormalizarTexto(alumno.ApellidoMaterno);
            }
            else
            {
                var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Id == alumno.UsuarioResponsableId);
                if (usuario == null)
                    return (false, "El usuario responsable no existe.");

                existente.Nombre = NormalizarTexto(usuario.Nombre);
                existente.ApellidoPaterno = NormalizarTexto(usuario.ApellidoPaterno);
                existente.ApellidoMaterno = NormalizarTexto(usuario.ApellidoMaterno);
            }

            await _context.SaveChangesAsync();

            return (true, "Alumno actualizado correctamente.");
        }

        public async Task<bool> ExisteAsync(int id)
        {
            return await _context.Alumnos.AnyAsync(a => a.Id == id);
        }

        private IQueryable<Alumno> AplicarBusqueda(IQueryable<Alumno> query, string? busqueda)
        {
            if (string.IsNullOrWhiteSpace(busqueda))
                return query;

            var texto = NormalizarTexto(busqueda);

            return query.Where(a =>
                EF.Functions.Like(a.Nombre, $"%{texto}%") ||
                EF.Functions.Like(a.ApellidoPaterno, $"%{texto}%") ||
                EF.Functions.Like(a.ApellidoMaterno, $"%{texto}%"));
        }

        private async Task<(bool Exitoso, string Mensaje)> ValidarAlumnoAsync(Alumno alumno, bool esEdicion, bool esUsuarioExterno)
        {
            if (string.IsNullOrWhiteSpace(alumno.UsuarioResponsableId))
                return (false, "El usuario responsable es obligatorio.");

            var usuarioResponsableExiste = await _context.Users
                .AnyAsync(u => u.Id == alumno.UsuarioResponsableId);

            if (!usuarioResponsableExiste)
                return (false, "El usuario responsable no existe.");

            if (alumno.ParentescoId <= 0)
                return (false, "El parentesco es obligatorio.");

            var parentescoExiste = await _context.Parentescos
                .AnyAsync(p => p.Id == alumno.ParentescoId);

            if (!parentescoExiste)
                return (false, "El parentesco seleccionado no existe.");

            if (esUsuarioExterno)
            {
                if (string.IsNullOrWhiteSpace(alumno.Nombre))
                    return (false, "El nombre es obligatorio.");

                if (string.IsNullOrWhiteSpace(alumno.ApellidoPaterno))
                    return (false, "El apellido paterno es obligatorio.");

                if (string.IsNullOrWhiteSpace(alumno.ApellidoMaterno))
                    return (false, "El apellido materno es obligatorio.");
            }

            if (alumno.FechaNacimiento == default)
                return (false, "La fecha de nacimiento es obligatoria.");

            if (alumno.FechaNacimiento.Date > DateTime.Today)
                return (false, "La fecha de nacimiento no puede ser mayor a la fecha actual.");

            return (true, "Validación correcta.");
        }

        private static string NormalizarTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            return string.Join(" ",
                texto.Trim()
                     .Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .ToUpperInvariant();
        }

        public async Task<(bool Exitoso, string Mensaje)> DesactivarAsync(int id)
        {
            if (id <= 0)
                return (false, "Id inválido.");

            var alumno = await _context.Alumnos
                .Include(a => a.Inscripciones)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (alumno == null)
                return (false, "El alumno no fue encontrado.");

            alumno.Estado = EstadoRegistro.Inactivo;
            await _context.SaveChangesAsync();

            return (true, "Alumno desactivado correctamente.");
        }

        public async Task<(bool Exitoso, string Mensaje)> ActivarAsync(int id)
        {
            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.Id == id);

            if (alumno == null)
                return (false, "El alumno no fue encontrado.");

            alumno.Estado = EstadoRegistro.Activo;
            await _context.SaveChangesAsync();

            return (true, "Alumno activado correctamente.");
        }


        public Task<(bool Exitoso, string Mensaje)> CrearAsync(Alumno alumno)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Exitoso, string Mensaje)> ActualizarAsync(Alumno alumno)
        {
            throw new NotImplementedException();
        }

        public Task<(bool Exitoso, string Mensaje)> EliminarAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}