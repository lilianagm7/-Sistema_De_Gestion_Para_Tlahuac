using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Catalogos;

namespace Sistema_Actividades_Tlahuac.Services.Catalogos
{
    public class ParentescoService
    {
        private readonly ApplicationDbContext _context;

        public ParentescoService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Obtener lista de parentescos
        public async Task<List<Parentesco>> ObtenerTodas(string? buscador, bool incluirInactivas = false)
        {
            var query = _context.Parentescos.AsQueryable();

            if (!incluirInactivas)
            {
                query = query.Where(c => c.Estado == EstadoRegistro.Activo);
            }
            if (!string.IsNullOrEmpty(buscador))
            {
                var busNormalizado = buscador.Trim().ToUpper();
                query = query.Where(c => c.Nombre.ToUpper().Contains(busNormalizado));
            }

            return await query
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        //Obtener por id
        public async Task<Parentesco?> ObtenerPorId(int id)
        {
            return await _context.Parentescos
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        //Normalizar nombres con acentos o mayuscula/miniscula
        private string Normalizar(string nombre)
        {
            return nombre.Trim().ToUpper();
        }

        //Validacion de duplicados con el mismo nombre
        public async Task<bool> ExisteNombre(string nombre, int? id = null)
        {
            var nombreNormalizado = Normalizar(nombre);

            return await _context.Parentescos
                .AnyAsync(c => c.Nombre.ToUpper().Trim() == nombreNormalizado
                            && (id == null || c.Id != id));
        }

        //Crear parentesco
        public async Task Crear(Parentesco parentesco)
        {
            //Normalizacion antes de guardar
            parentesco.Nombre = Normalizar(parentesco.Nombre);
            //estado inicial
            parentesco.Estado = EstadoRegistro.Activo;

            _context.Add(parentesco);
            await _context.SaveChangesAsync();
        }

        //Editar categoria
        public async Task Editar(Parentesco parentesco)
        {
            //Normalizacion para la consistencia
            parentesco.Nombre = Normalizar(parentesco.Nombre);

            _context.Update(parentesco);
            await _context.SaveChangesAsync();
        }

        //No eliminamos, solo desactivamos
        public async Task Desactivar(int id)
        {
            var parentesco = await _context.Parentescos.FindAsync(id);

            if (parentesco == null) return;

            parentesco.Estado = EstadoRegistro.Inactivo;
            await _context.SaveChangesAsync();
        }
    }
}
