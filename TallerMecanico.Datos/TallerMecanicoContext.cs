using Microsoft.EntityFrameworkCore;
using TallerMecanico.Entidades;

namespace TallerMecanico.Datos;

public partial class TallerMecanicoContext(DbContextOptions<TallerMecanicoContext> options) : DbContext(options)
{
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<OrdenServicio> OrdenesServicio { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<OrdenServicio>(entity =>
        {
            entity.ToTable("OrdenesServicio");
            entity.Property(e => e.DescripcionTrabajo).HasMaxLength(300);
            entity.Property(e => e.Estado).HasMaxLength(20);
            entity.Property(e => e.Placa).HasMaxLength(10);
            entity.HasOne(d => d.Cliente).WithMany(p => p.OrdenesServicio)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrdenesServicio_Clientes");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(e => e.Id).HasName("PK_Usuarios");
            entity.HasIndex(e => e.Correo).IsUnique().HasDatabaseName("UQ_Usuarios_Correo");
            entity.Property(e => e.Correo).HasMaxLength(254).IsRequired();
            entity.Property(e => e.ClaveHash).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Rol).HasMaxLength(30).IsRequired();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
