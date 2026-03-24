using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Data;
using Sistema_Actividades_Tlahuac.Models.Enums;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
namespace Sistema_Actividades_Tlahuac.Services.Catalogos
{
    public class EspacioService
    {
        private readonly ApplicationDbContext _context;

        public EspacioService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Organizar lista por nombre
        //Buscador 
        //Activas e incactivas
        public async Task<List<Espacio>> ObtenerTodas(string? buscador, bool incluirInactivas = false)
        {
            var query = _context.Espacios.AsQueryable();

            if (!incluirInactivas)
            {
                query = query.Where(e => e.Estado == EstadoRegistro.Activo);
            }

            if (!string.IsNullOrEmpty(buscador))
            {
                var busNormalizado = buscador.Trim().ToUpper();
                query = query.Where(e => e.Nombre.ToUpper().Contains(busNormalizado));
            }

            return await query
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<Espacio?> ObtenerPorId(int id)
        {
            return await _context.Espacios
                .FirstOrDefaultAsync(e => e.Id == id);
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

            return await _context.Espacios
                .AnyAsync(c => c.Nombre.ToUpper().Trim() == nombreNormalizado
                            && (id == null || c.Id != id));
        }

        //Crear espacio
        public async Task Create(Espacio espacio)
        {
            //Checamos que sea mayor de cero
            if (espacio.Capacidad <= 0)
            {
                throw new Exception("La capacidad debe ser mayor a 0");
            }
            //Checamos nombre no duplicado
            if (await ExisteNombre(espacio.Nombre))
                throw new Exception("Ya existe un espacio con ese nombre");
            //Normalizar antes de guardar
            espacio.Nombre = Normalizar(espacio.Nombre);
            //Estado inicial
            espacio.Estado = EstadoRegistro.Activo;
            //Guardamos
            _context.Espacios.Add(espacio);
            await _context.SaveChangesAsync();
        }

        //Editar espacio
        public async Task Edit(Espacio espacio)
        {
            //Valida el espacio mayor que cero
            if (espacio.Capacidad <= 0)
                throw new Exception("La capacidad debe ser mayor a 0");
            //Valida nombre existente
            if (await ExisteNombre(espacio.Nombre, espacio.Id))
                throw new Exception("Ya existe un espacio con ese nombre");
            //Normalizar para asegurar
            espacio.Nombre = Normalizar(espacio.Nombre);
            //Actualizamos
            _context.Update(espacio);
            await _context.SaveChangesAsync();
        }

        //Desactivamos (No eliminamos)
        public async Task Desactivar(int id)
        {
            var espacio = await _context.Espacios.FindAsync(id);
            if (espacio == null) return;
            espacio.Estado = EstadoRegistro.Inactivo;
            await _context.SaveChangesAsync();
        }


    }
}
