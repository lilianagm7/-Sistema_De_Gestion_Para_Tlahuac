using Sistema_Actividades_Tlahuac.Models.Alumnos;

namespace Sistema_Actividades_Tlahuac.Services.Actores
{
    public interface IAlumnoService
    {
        Task<List<Alumno>> ObtenerTodosAsync(string? busqueda = null);
        Task<List<Alumno>> ObtenerPorResponsableAsync(string usuarioResponsableId, string? busqueda = null);
        Task<Alumno?> ObtenerPorIdAsync(int id);
        Task<(bool Exitoso, string Mensaje)> CrearAsync(Alumno alumno, bool esUsuarioExterno);
        Task<(bool Exitoso, string Mensaje)> ActualizarAsync(Alumno alumno, bool esUsuarioExterno);
        Task<(bool Exitoso, string Mensaje)> DesactivarAsync(int id);
        Task<(bool Exitoso, string Mensaje)> ActivarAsync(int id);

        Task<bool> ExisteAsync(int id);
    }
}
