using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema_Actividades_Tlahuac.Models.Actores;
using Sistema_Actividades_Tlahuac.Models.Alumnos;
using Sistema_Actividades_Tlahuac.Models.Catalogos;
using Sistema_Actividades_Tlahuac.Models.Eventos;
using Sistema_Actividades_Tlahuac.Models.Inscripciones;
using Sistema_Actividades_Tlahuac.Models.Talleres;
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
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Taller> Talleres { get; set; }
        public DbSet<Instructor> Instructores { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }

        //NO permite que se borren datos en cascada por parte de inscripciones.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*          //NO permite que se borren datos en cascada por parte de Eventos.

                        modelBuilder.Entity<Evento>()
                            .HasOne(e => e.Instructor)
                            .WithMany(i => i.Eventos)
                            .HasForeignKey(e => e.InstructorId)
                            .OnDelete(DeleteBehavior.Restrict);
            */

            //Control de duplicados
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Parentesco>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Alumno>()
                .HasOne(a => a.UsuarioResponsable)
                .WithMany()
                .HasForeignKey(a => a.UsuarioResponsableId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Alumno>()
                .HasOne(a => a.Parentesco)
                .WithMany()
                .HasForeignKey(a => a.ParentescoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Usuario)
                .WithOne(u => u.Instructor)
                .HasForeignKey<Instructor>(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            //INSCRIPCIONES
            //NO permite que se borren datos en cascada por parte de alumnos e inscripciones.
            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Alumno)
                .WithMany(a => a.Inscripciones)
                .HasForeignKey(i => i.AlumnoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Evento)
                .WithMany()
                .HasForeignKey(i => i.EventoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Taller)
                .WithMany()
                .HasForeignKey(i => i.TallerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscripcion>()
                .HasIndex(i => new { i.AlumnoId, i.EventoId })
                .IsUnique()
                .HasFilter("[EventoId] IS NOT NULL");

            modelBuilder.Entity<Inscripcion>()
                .HasIndex(i => new { i.AlumnoId, i.TallerId })
                .IsUnique()
                .HasFilter("[TallerId] IS NOT NULL");

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.UsuarioCrea)
                .WithMany()
                .HasForeignKey(i => i.UsuarioCreacion)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Instructor>()
                .HasOne(i => i.Us_Modifica)
                .WithMany()
                .HasForeignKey(i => i.UsuarioModificacion)
                .OnDelete(DeleteBehavior.Restrict);
        }

        //Auditoria automatica, para todos los controladores
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            foreach (var entry in ChangeTracker.Entries())
            {
                //AUDITORIA PARA CREACIÓN (USUARIOS Y FECHAS)
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

                    if (entry.Entity is Parentesco parentesco)
                    {
                        parentesco.UsuarioCreacion = userId;
                        parentesco.FechaCreacion = DateTime.Now;
                    }

                    if (entry.Entity is Instructor instructor)
                    {
                        instructor.UsuarioCreacion = userId;
                        instructor.FechaCreacion = DateTime.Now;
                    }
                    if (entry.Entity is Alumno alumno)
                    {
                        alumno.UsuarioCreacion = userId;
                        alumno.FechaCreacion = DateTime.Now;
                    }

                    if (entry.Entity is Inscripcion inscripcion)
                    {
                        inscripcion.UsuarioCreacion = userId;
                        inscripcion.FechaCreacion = DateTime.Now;
                    }
                }

                //AUDITORIA PARA MODIFICACIÓN (USUARIOS Y FECHAS)
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

                    if (entry.Entity is Parentesco parentesco)
                    {
                        parentesco.UsuarioModificacion = userId;
                        parentesco.FechaModificacion = DateTime.Now;
                    }

                    if (entry.Entity is Instructor instructor)
                    {
                        instructor.UsuarioModificacion = userId;
                        instructor.FechaModificacion = DateTime.Now;
                    }
                    if (entry.Entity is Alumno alumno)
                    {
                        alumno.UsuarioModificacion = userId;
                        alumno.FechaModificacion = DateTime.Now;
                    }

                    if (entry.Entity is Inscripcion inscripcion)
                    {
                        inscripcion.UsuarioModificacion = userId;
                        inscripcion.FechaModificacion = DateTime.Now;
                    }

                    //PROTECCION AL MOMENTO DE CAMBIOS
                    if (entry.Entity is ApplicationUser)
                    {
                        continue;
                    }
                    else
                    {
                        entry.Property("FechaCreacion").IsModified = false;
                        entry.Property("UsuarioCreacion").IsModified = false;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}