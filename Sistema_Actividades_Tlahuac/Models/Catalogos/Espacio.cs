using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sistema_Actividades_Tlahuac.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Actividades_Tlahuac.Models.Catalogos
{
    public class Espacio
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;


        [Range(1, 10000, ErrorMessage = "La capacidad debe ser mayor a 0")]
        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Display(Name = "Capacidad maxima")]
        public int Capacidad { get; set; }

        //Registro historico
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;
        public DateTime FechaCreacion { get; set; }
        public string? UsuarioCreacion { get; set; }


        //Relaciones a otras tablas
        [Required(ErrorMessage = "El lugar es obligatorio")]
        [Display(Name = "Lugar al que corresponde")]
        public int LugarId { get; set; }
        [ValidateNever]
        public Lugar Lugar { get; set; }

        /*// Relación: en un espacio pueden realizarse eventos y talleres
        public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
        public ICollection<Taller> Talleres { get; set; } = new List<Taller>();
        */
    }
}
