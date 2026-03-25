using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Catalogos;

namespace Sistema_Actividades_Tlahuac.Services.Catalogos
{
    public class LugarService
    {
        private readonly ApplicationDbContext _context;

        public LugarService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Organizar lista por nombre
        //Activas e incactivas
        public async Task<List<Lugar>> ObtenerTodos(string? buscador, bool incluirInactivos = false)
        {
            var query = _context.Lugares.AsQueryable();

            if (!incluirInactivos)
            {
                query = query.Where(l => l.Estado == EstadoRegistro.Activo);
            }

            if (!string.IsNullOrEmpty(buscador))
            {
                var busNormalizado = buscador.Trim().ToUpper();
                query = query.Where(l =>
                    l.Nombre.ToUpper().Contains(busNormalizado) ||
                    l.Colonia.ToUpper().Contains(busNormalizado)
                );
            }

            return await query
                .OrderBy(l => l.Nombre)
                .ToListAsync();
        }
        //Buscador 
        public async Task<Lugar?> ObtenerPorId(int id)
        {
            return await _context.Lugares
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        //Normalizar nombres con acentos o mayuscula/miniscula
        private string Normalizar(string texto)
        {
            return texto.Trim().ToUpper();
        }


        //Validacion de duplicados con el mismo nombre y colonia 
        public async Task<bool> ExisteLugar(string nombre, string colonia, int? id = null)
        {
            var nombreNormalizado = Normalizar(nombre);
            var coloniaNormalizada = Normalizar(colonia);

            return await _context.Lugares
                .AnyAsync(l =>
                    l.Nombre.ToUpper().Trim() == nombreNormalizado &&
                    l.Colonia.ToUpper().Trim() == coloniaNormalizada &&
                    (id == null || l.Id != id)
                );
        }

        //Funcion de crear lugar
        public async Task Create(Lugar lugar)
        {
            if (await ExisteLugar(lugar.Nombre, lugar.Colonia))
                throw new Exception("Ya existe un lugar con ese nombre en esa colonia.");

            lugar.Nombre = Normalizar(lugar.Nombre);
            lugar.Colonia = Normalizar(lugar.Colonia);
            lugar.Estado = EstadoRegistro.Activo;

            _context.Lugares.Add(lugar);
            await _context.SaveChangesAsync();
        }


        //Funcion de editar lugar
        public async Task Edit(Lugar lugar)
        {
            if (await ExisteLugar(lugar.Nombre, lugar.Colonia, lugar.Id))
                throw new Exception("Ya existe un lugar con ese nombre en esa colonia.");

            lugar.Nombre = Normalizar(lugar.Nombre);
            lugar.Colonia = Normalizar(lugar.Colonia);

            _context.Lugares.Update(lugar);
            await _context.SaveChangesAsync();
        }

        //Funcion de desactivar lugar
        public async Task Desactivar(int id)
        {
            var lugar = await _context.Lugares.FindAsync(id);

            if (lugar == null) return;

            lugar.Estado = EstadoRegistro.Inactivo;

            await _context.SaveChangesAsync();
        }

    }
}
