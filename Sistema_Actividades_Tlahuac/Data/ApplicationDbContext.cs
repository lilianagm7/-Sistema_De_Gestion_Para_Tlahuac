using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Sistema_Actividades_Tlahuac.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor
        ) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Relacion a las tablas en SQL Server
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Espacio> Espacios { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<Parentesco> Parentescos { get; set; }


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

        //Auditoria automatica, para todos mis controladores
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            foreach (var entry in ChangeTracker.Entries())
            {
                //AUDITORIA PARA CREACIÓN (UDUARIOS Y FECHAS)
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is Espacio espacio)
                    {
                        espacio.UsuarioCreacion = userId;
                        espacio.FechaCreacion = DateTime.Now;
                    }

                    if (entry.Entity is Lugar lugar)
                    {
                        lugar.UsuarioCreacion = userId;
                        lugar.FechaCreacion = DateTime.Now;
                    }

                    if (entry.Entity is Categoria categoria)
                    {
                        categoria.UsuarioCreacion = userId;
                        categoria.FechaCreacion = DateTime.Now;
                    }
                }

                //AUDITORIA PARA MODIFICACIÓN (UDUARIOS Y FECHAS)
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is Espacio espacio)
                    {
                        espacio.UsuarioModificacion = userId;
                        espacio.FechaModificacion = DateTime.Now;
                    }

                    if (entry.Entity is Lugar lugar)
                    {
                        lugar.UsuarioModificacion = userId;
                        lugar.FechaModificacion = DateTime.Now;
                    }

                    if (entry.Entity is Categoria categoria)
                    {
                        categoria.UsuarioModificacion = userId;
                        categoria.FechaModificacion = DateTime.Now;
                    }

                    //PROTECCION AL MOMENTO DE CAMBIOS
                    entry.Property("FechaCreacion").IsModified = false;
                    entry.Property("UsuarioCreacion").IsModified = false;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }


    }
}