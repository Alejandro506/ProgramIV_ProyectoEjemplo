using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace myFirstProject.Data.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animale> Animales { get; set; }
    public virtual DbSet<Cita> Citas { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Doctore> Doctores { get; set; }
    public virtual DbSet<Ejemplare> Ejemplares { get; set; }
    public virtual DbSet<Estudiante> Estudiantes { get; set; }
    public virtual DbSet<HistorialCita> HistorialCitas { get; set; }
    public virtual DbSet<HorariosDoctor> HorariosDoctors { get; set; }
    public virtual DbSet<Libro> Libros { get; set; }
    public virtual DbSet<Libros2> Libros2s { get; set; }
    public virtual DbSet<Prestamo> Prestamos { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RolesPermiso> RolesPermisos { get; set; }
    public virtual DbSet<TiposAnimale> TiposAnimales { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }
    public virtual DbSet<Vista_Animales_Cita> Vista_Animales_Citas { get; set; }


    // === VISTAS ===
    // Deja SOLO este DbSet para la vista (el plural causaba conflicto)
    public virtual DbSet<Vista_HistorialCitasDetalle> Vista_HistorialCitasDetalles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tu esquema por defecto
        modelBuilder.HasDefaultSchema("PrograAvanzada202502User06");

        modelBuilder.Entity<Animale>(entity =>
        {
            entity.HasKey(e => e.IdAnimal).HasName("PK__Animales__951092F03EFDEE18");

            entity.Property(e => e.IdAnimal).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(false);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Animales)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Animal_Cliente");

            entity.HasOne(d => d.IdTipoNavigation).WithMany(p => p.Animales)
                .HasForeignKey(d => d.IdTipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Animal_Tipo");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Citas__394B0202A94610DA");

            entity.Property(e => e.IdCita).ValueGeneratedNever();
            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");

            entity.HasOne(d => d.IdAnimalNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdAnimal)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Animal");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cita_Doctor");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D594664249A34F2A");

            entity.Property(e => e.IdCliente).ValueGeneratedNever();
            entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Telefono).HasMaxLength(20).IsUnicode(false);
        });

        modelBuilder.Entity<Doctore>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("PK__Doctores__F838DB3E425C6716");

            entity.Property(e => e.IdDoctor).ValueGeneratedNever();
            entity.Property(e => e.Especialidad).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(false);
        });

        modelBuilder.Entity<Ejemplare>(entity =>
        {
            entity.HasKey(e => e.EjemplarId).HasName("PK__Ejemplar__C7803E4927B893C2");

            entity.HasIndex(e => e.CodigoInventario, "UQ__Ejemplar__D5EC1A77E68D5095").IsUnique();

            entity.Property(e => e.CodigoInventario).HasMaxLength(100);
            entity.Property(e => e.Estado).HasMaxLength(50).HasDefaultValue("Disponible");

            entity.HasOne(d => d.Libro).WithMany(p => p.Ejemplares)
                .HasForeignKey(d => d.LibroId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ejemplare__Libro__42ACE4D4");
        });

        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.HasKey(e => e.EstudianteId).HasName("PK__Estudian__6F7682D8AA5A0EB2");

            entity.HasIndex(e => e.Carnet, "UQ__Estudian__5E387B4DA9311393").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Carnet).HasMaxLength(50);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<HistorialCita>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__Historia__9CC7DBB43A8C9B04");

            entity.Property(e => e.IdHistorial).ValueGeneratedNever();
            entity.Property(e => e.Accion).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.FechaAccion).HasDefaultValueSql("(getdate())").HasColumnType("datetime");
            entity.Property(e => e.Observaciones).HasMaxLength(255).IsUnicode(false);

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.HistorialCita)
                .HasForeignKey(d => d.IdCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Historial_Cita");
        });

        modelBuilder.Entity<HorariosDoctor>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horarios__1539229B28961D8F");

            entity.ToTable("HorariosDoctor");

            entity.Property(e => e.IdHorario).ValueGeneratedNever();
            entity.Property(e => e.DiaSemana).HasMaxLength(15).IsUnicode(false);

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.HorariosDoctors)
                .HasForeignKey(d => d.IdDoctor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Horario_Doctor");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.LibroId).HasName("PK__Libros__35A1ECED30696DED");

            entity.Property(e => e.Autor).HasMaxLength(255);
            entity.Property(e => e.Genero).HasMaxLength(100);
            entity.Property(e => e.Titulo).HasMaxLength(255);
        });

        modelBuilder.Entity<Libros2>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Libros2__3214EC077F90D7BD");

            entity.ToTable("Libros2");

            entity.Property(e => e.Edition).HasMaxLength(50);
            entity.Property(e => e.Editor).HasMaxLength(255);
            entity.Property(e => e.OriginalName).HasMaxLength(255);
            entity.Property(e => e.SpanishName).HasMaxLength(255);
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.HasKey(e => e.PrestamoId).HasName("PK__Prestamo__AA58A0A097BDCF9A");

            entity.Property(e => e.FechaDevolucion).HasColumnType("datetime");
            entity.Property(e => e.FechaPrestamo).HasDefaultValueSql("(getdate())").HasColumnType("datetime");

            entity.HasOne(d => d.Ejemplar).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.EjemplarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamos__Ejemp__4A4E069C");

            entity.HasOne(d => d.Estudiante).WithMany(p => p.Prestamos)
                .HasForeignKey(d => d.EstudianteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Prestamos__Estud__4959E263");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C25B955D3");

            entity.Property(e => e.IdRol).ValueGeneratedNever();
            entity.Property(e => e.NombreRol).HasMaxLength(50).IsUnicode(false);
        });

        modelBuilder.Entity<RolesPermiso>(entity =>
        {
            entity.HasKey(e => new { e.IdRol, e.Accion });

            entity.Property(e => e.Accion).HasMaxLength(20).IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.RolesPermisos)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RolesPermisos_Rol");
        });

        modelBuilder.Entity<TiposAnimale>(entity =>
        {
            entity.HasKey(e => e.IdTipo).HasName("PK__TiposAni__9E3A29A5BB30A95F");

            entity.Property(e => e.IdTipo).ValueGeneratedNever();
            entity.Property(e => e.Descripcion).HasMaxLength(50).IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF974EF9FC0B");

            entity.Property(e => e.IdUsuario).ValueGeneratedNever();
            entity.Property(e => e.NombreUsuario).HasMaxLength(100).IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_Usuario_Rol");
        });

        // ====== MAPEOS DE VISTAS ======

        modelBuilder.Entity<Vista_Animales_Cita>(entity =>
        {
            entity.HasNoKey();
            // OJO: nombre correcto y esquema por defecto ya configurado arriba
            entity.ToView("Vista_Animales_Citas", "PrograAvanzada202502User06");

            entity.Property(e => e.Estado).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.NombreAnimal).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.NombreCliente).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.NombreDoctor).HasMaxLength(100).IsUnicode(false);
        });

        modelBuilder.Entity<Vista_HistorialCitasDetalle>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("Vista_HistorialCitasDetalle");

            entity.Property(e => e.Accion).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.EstadoCita).HasMaxLength(20).IsUnicode(false);
            entity.Property(e => e.FechaAccion).HasColumnType("datetime");
            entity.Property(e => e.FechaHoraCita).HasColumnType("datetime");
            entity.Property(e => e.NombreAnimal).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.NombreDoctor).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Observaciones).HasMaxLength(255).IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
