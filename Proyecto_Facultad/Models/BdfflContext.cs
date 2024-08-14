using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Facultad.Models;

public partial class BdfflContext : DbContext
{
    public BdfflContext()
    {
    }

    public BdfflContext(DbContextOptions<BdfflContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumnos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno).HasName("PK__Alumno__6D77A7F1457A5B56");

            entity.ToTable("Alumno");

            entity.HasIndex(e => e.CodigoFrater, "IDX_Alumno_CodigoFrater");

            entity.HasIndex(e => e.EstatusAlumno, "IDX_Alumno_Estatus_alumno");

            entity.HasIndex(e => e.FechaBautizo, "IDX_Alumno_Fecha_bautizo");

            entity.HasIndex(e => e.FechaNacimiento, "IDX_Alumno_Fecha_nacimiento");

            entity.HasIndex(e => e.PrimerApellidoAlumno, "IDX_Alumno_PrimerApellido");

            entity.HasIndex(e => e.PrimerNombreAlumno, "IDX_Alumno_PrimerNombre");

            entity.HasIndex(e => e.CodigoFrater, "UQ__Alumno__F8C2B3A9653E0236").IsUnique();

            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.ApellidoCasado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Apellido_casado");
            entity.Property(e => e.Becado).HasColumnName("becado");
            entity.Property(e => e.CodigoFrater)
                .HasMaxLength(20)
                .HasColumnName("Codigo_Frater");
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EstadoCivil)
                .HasMaxLength(20)
                .HasColumnName("Estado_civil");
            entity.Property(e => e.EstatusAlumno).HasColumnName("Estatus_alumno");
            entity.Property(e => e.FechaBautizo).HasColumnName("Fecha_bautizo");
            entity.Property(e => e.FechaNacimiento).HasColumnName("Fecha_nacimiento");
            entity.Property(e => e.NumeroCelula)
                .HasMaxLength(20)
                .HasColumnName("Numero_celula");
            entity.Property(e => e.OtrosNombresAlumno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Otros_nombres_alumno");
            entity.Property(e => e.PrimerApellidoAlumno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PrimerApellido_alumno");
            entity.Property(e => e.PrimerNombreAlumno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PrimerNombre_alumno");
            entity.Property(e => e.SegundoApellidoAlumno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SegundoApellido_alumno");
            entity.Property(e => e.SegundoNombreAlumno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("SegundoNombre_alumno");
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
