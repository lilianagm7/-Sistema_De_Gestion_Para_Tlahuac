using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Catalogos;


namespace Sistema_Actividades_Tlahuac.Services.Catalogos
{
    public class CategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
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

            return await _context.Categorias
                .AnyAsync(c => c.Nombre.ToUpper().Trim() == nombreNormalizado
                            && (id == null || c.Id != id));
        }

        //Crear categoria
        public async Task Crear(Categoria categoria)
        {
            //Normalizacion antes de guardar
            categoria.Nombre = Normalizar(categoria.Nombre);
            //estado inicial
            categoria.Estado = EstadoRegistro.Activo;

            _context.Add(categoria);
            await _context.SaveChangesAsync();
        }

        //Editar categoria
        public async Task Editar(Categoria categoria)
        {
            //Normalizacion para la consistencia
            categoria.Nombre = Normalizar(categoria.Nombre);

            _context.Update(categoria);
            await _context.SaveChangesAsync();
        }

        //No eliminamos, solo desactivamos
        public async Task Desactivar(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null) return;

            categoria.Estado = EstadoRegistro.Inactivo;
            await _context.SaveChangesAsync();
        }
        //Obtener lista de categorias
        public async Task<List<Categoria>> ObtenerTodas(string? buscador, bool incluirInactivas = false)
        {
            var query = _context.Categorias.AsQueryable();

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

        public async Task<Categoria?> ObtenerPorId(int id) { 
            return await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
