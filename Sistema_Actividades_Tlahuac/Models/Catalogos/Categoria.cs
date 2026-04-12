using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Sistema_Actividades_Tlahuac.Models.Actores;
using System.ComponentModel.DataAnnotations.Schema;
using Sistema_Actividades_Tlahuac.Models.Enums;

namespace Sistema_Actividades_Tlahuac.Models.Catalogos
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Máximo 250 caracteres")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        //Conexion con el enum de estados
        public EstadoRegistro Estado { get; set; } = EstadoRegistro.Activo;
        
        //Registro historico
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        //Relacion con la tabla usuarios
        public string? UsuarioCreacion { get; set; }
        [ForeignKey("UsuarioCreacion")]
        [ValidateNever]
        public ApplicationUser Usuario { get; set; }
        //Modificacion
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        [ForeignKey("UsuarioModificacion")]
        [ValidateNever]
        public ApplicationUser Us_Modifica { get; set; }

        // Relación donde categoria puede tener muchos eventos
        //public ICollection<Evento>? Eventos { get; set; } = new List<Evento>();

    }
}
