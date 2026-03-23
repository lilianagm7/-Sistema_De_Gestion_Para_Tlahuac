using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using System.Reflection.Emit;

namespace Sistema_Actividades_Tlahuac.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //Relacion a las tablas en SQL Server
        public DbSet<Categoria> Categorias { get; set; }

        //NO permite que se borren datos en cascada por parte de inscripciones.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
/*          modelBuilder.Entity<Inscripcion>()
                 .HasOne(i => i.UsuarioRegistro)
                 .WithMany()
                 .HasForeignKey(i => i.UsuarioRegistroId)
                 .OnDelete(DeleteBehavior.Restrict); //Restringe borrar registros
*/
            
/*            //NO permite que se borren datos en cascada por parte de Eventos.

            modelBuilder.Entity<Evento>()
                .HasOne(e => e.Instructor)
                .WithMany(i => i.Eventos)
                .HasForeignKey(e => e.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);
*/
            //Control de duplicados en categorias
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();
        }
        
    }
}