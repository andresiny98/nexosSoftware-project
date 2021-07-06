using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace bibliotecaNexos.Models
{
    public partial class dbnexosContext : DbContext
    {
        public dbnexosContext()
        {
        }

        public dbnexosContext(DbContextOptions<dbnexosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Autor> Autors { get; set; }
        public virtual DbSet<Editorial> Editorials { get; set; }
        public virtual DbSet<Libro> Libroes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=NexosDataBase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.IdAutor)
                    .HasName("PK__AUTOR__DA37C13713F1800A");

                entity.ToTable("AUTOR");

                entity.Property(e => e.IdAutor)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_AUTOR");

                entity.Property(e => e.CiudadProcedencia)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("CIUDAD_PROCEDENCIA");

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("CORREO_ELECTRONICO");

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("FECHA_NACIMIENTO");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");
            });

            modelBuilder.Entity<Editorial>(entity =>
            {
                entity.HasKey(e => e.IdEditorial)
                    .HasName("PK__EDITORIA__EA05B1D748EECCC1");

                entity.ToTable("EDITORIAL");

                entity.Property(e => e.IdEditorial)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_EDITORIAL");

                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("CORREO_ELECTRONICO");

                entity.Property(e => e.DireccionCorrespondencia)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("DIRECCION_CORRESPONDENCIA");

                entity.Property(e => e.MaximoLibros).HasColumnName("MAXIMO_LIBROS");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("NOMBRE");

                entity.Property(e => e.Telefono).HasColumnName("TELEFONO");
            });

            modelBuilder.Entity<Libro>(entity =>
            {
                entity.HasKey(e => e.IdLibro)
                    .HasName("PK__LIBRO__93FF0A061AACB099");

                entity.ToTable("LIBRO");

                entity.Property(e => e.IdLibro)
                    .ValueGeneratedNever()
                    .HasColumnName("ID_LIBRO");

                entity.Property(e => e.Año).HasColumnName("AÑO");

                entity.Property(e => e.IdAutor).HasColumnName("ID_AUTOR");

                entity.Property(e => e.IdEditorial).HasColumnName("ID_EDITORIAL");

                entity.Property(e => e.Paginas).HasColumnName("PAGINAS");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("TITULO");

                entity.HasOne(d => d.IdAutorNavigation)
                    .WithMany(p => p.Libroes)
                    .HasForeignKey(d => d.IdAutor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LIBRO__ID_AUTOR__38996AB5");

                entity.HasOne(d => d.IdEditorialNavigation)
                    .WithMany(p => p.Libroes)
                    .HasForeignKey(d => d.IdEditorial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LIBRO__ID_EDITOR__37A5467C");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
