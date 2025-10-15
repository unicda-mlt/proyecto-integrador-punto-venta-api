using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rol>(entity =>
            {
                entity.ToTable("roles");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id);
                entity.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(x => x.CreadoEn).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(x => x.ActualizadoEn);

                entity.HasData(new Rol {
                    Id=1,
                    Nombre="ADMIN"
                });
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuarios");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id);
                entity.Property(x => x.RolId).IsRequired();
                entity.Property(x => x.Nombre).IsRequired().HasMaxLength(150);
                entity.Property(x => x.UsuarioNombre).IsRequired().HasMaxLength(30);
                entity.Property(x => x.Password).IsRequired().HasMaxLength(1000);
                entity.Property(x => x.Activo).IsRequired();
                entity.Property(x => x.Eliminado).IsRequired();
                entity.Property(x => x.CreadoEn).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(x => x.ActualizadoEn);

                entity.HasOne(x => x.Rol)
                      .WithMany(x => x.Usuarios)
                      .HasForeignKey(x => x.RolId)
                      .HasConstraintName("usuarios_fk_rol_id");

                entity.HasIndex(x => x.UsuarioNombre)
                    .HasDatabaseName("udx_usuario_nombre")
                    .IsUnique()
                    .HasFilter("[Eliminado] = 0");

                entity.HasIndex(x => new { x.Eliminado, x.Activo })
                    .HasDatabaseName("idx_usuario_eliminado_activo");

                entity.HasData(new Usuario()
                {
                    Id = new Guid("bfe03e22-65e4-4007-8420-07c1b53c4726"),
                    RolId = 1,
                    Nombre = "Admin",
                    UsuarioNombre = "admin",
                    Password = "9U0zeOGybSi5hk81k/nFzw==.FN5jpe1k2hAMfU0SIg2QuTiwVdhsFdYsC1ykHHAwkzk=",
                    Activo = true,
                    Eliminado = false
                });
            });
        }
    }
}
