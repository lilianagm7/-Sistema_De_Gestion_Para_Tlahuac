using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Enums;

namespace Sistema_Actividades_Tlahuac.Services.Instructores
{
    public class InstructorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;


        public InstructorService(ApplicationDbContext context, IWebHostEnvironment env, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        //Normalizar nombres con acentos o mayuscula/miniscula
        private string Normalizar(string nombre)
        {
            return nombre.Trim().ToUpper();
        }

        //Validar duplicado por usuario
        public async Task<bool> ExisteInstructorPorUsuario(string userId)
        {
            return await _context.Instructores
                .AnyAsync(i => i.UserId == userId);
        }

        //Validar duplicado por RFC
        public async Task<bool> ExisteRFC(string rfc, int? id = null)
        {
            var rfcNorm = Normalizar(rfc);

            return await _context.Instructores
                .AnyAsync(i =>
                    i.RFC.ToUpper().Trim() == rfcNorm &&
                    (id == null || i.Id != id));
        }

        //Crear nuevo instructor
        public async Task Crear(Instructor instructor, string usuarioId)
        {
            if (await ExisteInstructorPorUsuario(instructor.UserId))
                throw new Exception("Este usuario ya está registrado como instructor");

            if (await ExisteRFC(instructor.RFC))
                throw new Exception("Ya existe un instructor con ese RFC");
            var user = await _userManager.FindByIdAsync(instructor.UserId);

            if (user == null)
                throw new Exception("Usuario no válido");

            if (!await _userManager.IsInRoleAsync(user, "Instructor"))
            {
                await _userManager.AddToRoleAsync(user, "Instructor");
            }

            instructor.RFC = Normalizar(instructor.RFC);
            instructor.Especialidad = Normalizar(instructor.Especialidad);
            instructor.UsuarioCreacion = usuarioId;
            instructor.FechaCreacion = DateTime.UtcNow;
            instructor.Estado = EstadoRegistro.Activo;
            _context.Instructores.Add(instructor);
            await _context.SaveChangesAsync();
        }

        //Editar instructor
        public async Task Editar(Instructor model, string usuarioId)
        {
            var instructor = await _context.Instructores.FindAsync(model.Id);

            if (instructor == null)
                throw new Exception("Instructor no encontrado");

            if (await ExisteRFC(model.RFC, model.Id))
                throw new Exception("Ya existe un instructor con ese RFC");

            instructor.RFC = Normalizar(model.RFC);
            instructor.Direccion = model.Direccion;
            instructor.NivelAcademico = model.NivelAcademico;
            instructor.DescripcionGrado = model.DescripcionGrado;
            instructor.Especialidad = Normalizar(model.Especialidad);
            instructor.TipoContrato = model.TipoContrato;
            instructor.Salario = model.Salario;
            instructor.FechaIngreso = model.FechaIngreso;
            instructor.FechaFinContrato = model.FechaFinContrato;
            instructor.Telefono = model.Telefono;
            instructor.EmailContacto = model.EmailContacto;

            instructor.UsuarioModificacion = usuarioId;
            instructor.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
        //Desactivar
        public async Task Desactivar(int id, string usuarioId)
        {
            var instructor = await _context.Instructores.FindAsync(id);

            if (instructor == null)
                throw new Exception("Instructor no encontrado");

            instructor.Estado = EstadoRegistro.Inactivo;
            instructor.UsuarioModificacion = usuarioId;
            instructor.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        //Obtener lista
        public async Task<List<Instructor>> ObtenerTodos(string? buscador, bool mostrarInactivos)
        {
            var query = _context.Instructores
                .Include(i => i.Usuario)
                .AsQueryable();

            if (!mostrarInactivos)
                query = query.Where(i => i.Estado == EstadoRegistro.Activo);

            if (!string.IsNullOrEmpty(buscador))
            {
                buscador = buscador.ToUpper();

                query = query.Where(i =>
                    i.RFC.ToUpper().Contains(buscador) ||
                    i.Especialidad.ToUpper().Contains(buscador) ||
                    i.Usuario.NombreCompleto.ToUpper().Contains(buscador));
            }

            return await query
                .OrderBy(i => i.Usuario.Nombre)
                .ThenBy(i => i.Usuario.ApellidoPaterno)
                .ThenBy(i => i.Usuario.ApellidoMaterno)
                .ToListAsync();
        }

        //Obtener por id
        public async Task<Instructor?> ObtenerPorId(int id)
        {
            return await _context.Instructores
                .Include(i => i.Usuario)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        //Guardar foto
        public async Task<string?> GuardarFoto(IFormFile? archivo)
        {
            if (archivo == null) return null;

            if (archivo.Length > 2 * 1024 * 1024)
                throw new Exception("La imagen no debe superar los 2MB");

            var extension = Path.GetExtension(archivo.FileName).ToLower();
            var permitidas = new[] { ".jpg", ".jpeg", ".png" };

            if (!permitidas.Contains(extension))
                throw new Exception("Formato no permitido");

            var carpeta = Path.Combine(_env.WebRootPath, "imagenes/instructores");

            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);

            var nombre = Guid.NewGuid() + extension;
            var ruta = Path.Combine(carpeta, nombre);

            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            return "/imagenes/instructores/" + nombre;
        }

        //Solo usuarios que no tienen rol asignado
        public async Task<List<SelectListItem>> ObtenerUsuariosDisponibles()
        {
            var usuariosConInstructor = await _context.Instructores
                .Select(i => i.UserId)
                .ToListAsync();

            var listaUsuarios = await _userManager.Users.ToListAsync();

            var usuarios = new List<SelectListItem>();

            foreach (var user in listaUsuarios)
            {
                if (usuariosConInstructor.Contains(user.Id))
                    continue;

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrador") || roles.Contains("Coordinador"))
                    continue;

                usuarios.Add(new SelectListItem
                {
                    Value = user.Id,
                    Text = user.Email + " (" + user.Nombre + " " + user.ApellidoPaterno + " " +user.ApellidoMaterno + ")"
                });
            }

            return usuarios;
        }
    }
}
