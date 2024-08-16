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

    public virtual DbSet<AsistenciaAlumno> AsistenciaAlumnos { get; set; }

    public virtual DbSet<AsistenciaStaff> AsistenciaStaffs { get; set; }

    public virtual DbSet<Bimestre> Bimestres { get; set; }

    public virtual DbSet<Capacitacion> Capacitacions { get; set; }

    public virtual DbSet<Horario> Horarios { get; set; }

    public virtual DbSet<Jornadum> Jornada { get; set; }

    public virtual DbSet<Leccion> Leccions { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Modulo> Modulos { get; set; }

    public virtual DbSet<ModulosRole> ModulosRoles { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<Notum> Nota { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

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

        modelBuilder.Entity<AsistenciaAlumno>(entity =>
        {
            entity.HasKey(e => new { e.IdAsistencia, e.IdAlumno }).HasName("PK__Asistenc__D69230E56374CD97");

            entity.ToTable("AsistenciaAlumno");

            entity.Property(e => e.IdAsistencia).HasColumnName("id_asistencia");
            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.IdAsistenciaStaff).HasColumnName("id_asistencia_staff");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.AsistenciaAlumnos)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_al__71BCD978");

            entity.HasOne(d => d.IdAsistenciaStaffNavigation).WithMany(p => p.AsistenciaAlumnos)
                .HasForeignKey(d => d.IdAsistenciaStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_as__70C8B53F");
        });

        modelBuilder.Entity<AsistenciaStaff>(entity =>
        {
            entity.HasKey(e => e.IdAsistenciaStaff).HasName("PK__Asistenc__4E497B353ECB0FE1");

            entity.ToTable("AsistenciaStaff");

            entity.Property(e => e.IdAsistenciaStaff)
                .ValueGeneratedNever()
                .HasColumnName("id_asistencia_staff");
            entity.Property(e => e.FechaClase).HasColumnName("fecha_clase");
            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.IdLeccion).HasColumnName("id_leccion");
            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");

            entity.HasOne(d => d.IdBimestreNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdBimestre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_bi__6CF8245B");

            entity.HasOne(d => d.IdLeccionNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdLeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_le__6C040022");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_me__6DEC4894");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_st__6B0FDBE9");
        });

        modelBuilder.Entity<Bimestre>(entity =>
        {
            entity.HasKey(e => e.IdBimestre).HasName("PK__Bimestre__7F19856521ED5BDD");

            entity.ToTable("Bimestre");

            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.NombreBimestre)
                .HasMaxLength(50)
                .HasColumnName("nombre_bimestre");
        });

        modelBuilder.Entity<Capacitacion>(entity =>
        {
            entity.HasKey(e => e.IdCapacitacion).HasName("PK__Capacita__FA471D9BD06D2725");

            entity.ToTable("Capacitacion");

            entity.Property(e => e.IdCapacitacion).HasColumnName("id_capacitacion");
            entity.Property(e => e.FechaCapacitacion).HasColumnName("fecha_capacitacion");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.Capacitacions)
                .HasForeignKey(d => d.IdStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Capacitac__id_st__795DFB40");
        });

        modelBuilder.Entity<Horario>(entity =>
        {
            entity.HasKey(e => e.IdHorario).HasName("PK__Horario__C5836D6938BC96F9");

            entity.ToTable("Horario");

            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.DiaHorario)
                .HasMaxLength(50)
                .HasColumnName("dia_horario");
            entity.Property(e => e.NombreHorario)
                .HasMaxLength(50)
                .HasColumnName("nombre_horario");
        });

        modelBuilder.Entity<Jornadum>(entity =>
        {
            entity.HasKey(e => e.IdJornada).HasName("PK__Jornada__6BD46D1AB0FC920B");

            entity.Property(e => e.IdJornada).HasColumnName("id_jornada");
            entity.Property(e => e.DiaJornada)
                .HasMaxLength(50)
                .HasColumnName("Dia_jornada");
            entity.Property(e => e.HorarioJornada)
                .HasMaxLength(50)
                .HasColumnName("Horario_jornada");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Jornada)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jornada__id_sede__58F12BAE");
        });

        modelBuilder.Entity<Leccion>(entity =>
        {
            entity.HasKey(e => e.IdLeccion).HasName("PK__Leccion__2015E24C8D636BA6");

            entity.ToTable("Leccion");

            entity.Property(e => e.IdLeccion).HasColumnName("id_leccion");
            entity.Property(e => e.IdLibro).HasColumnName("id_libro");
            entity.Property(e => e.NombreLeccion)
                .HasMaxLength(50)
                .HasColumnName("nombre_leccion");

            entity.HasOne(d => d.IdLibroNavigation).WithMany(p => p.Leccions)
                .HasForeignKey(d => d.IdLibro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leccion__id_libr__5F9E293D");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.IdLibro).HasName("PK__Libro__EC09C24EE0790F37");

            entity.ToTable("Libro");

            entity.Property(e => e.IdLibro).HasColumnName("id_libro");
            entity.Property(e => e.NombreLibro)
                .HasMaxLength(50)
                .HasColumnName("nombre_libro");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.IdMesa).HasName("PK__Mesa__68A1E15928ADDBBE");

            entity.ToTable("Mesa");

            entity.HasIndex(e => e.EstadoMesa, "IDX_Mesa_EstadoMesa");

            entity.HasIndex(e => e.FechaInicio, "IDX_Mesa_FechaInicio");

            entity.HasIndex(e => e.IdAlumno, "IDX_id_alumno");

            entity.HasIndex(e => e.IdHorario, "IDX_id_horario");

            entity.HasIndex(e => e.IdStaff, "IDX_id_staff");

            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");
            entity.Property(e => e.AnioAsignacion).HasColumnName("anio_asignacion");
            entity.Property(e => e.EstadoMesa).HasColumnName("Estado_mesa");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_inicio");
            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.IdHorario).HasColumnName("id_horario");
            entity.Property(e => e.IdJornada).HasColumnName("id_jornada");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mesa__id_alumno__65570293");

            entity.HasOne(d => d.IdHorarioNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdHorario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mesa__id_horario__68336F3E");

            entity.HasOne(d => d.IdJornadaNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdJornada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mesa__id_jornada__664B26CC");

            entity.HasOne(d => d.IdNivelNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdNivel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mesa__id_nivel__673F4B05");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mesa__id_staff__6462DE5A");
        });

        modelBuilder.Entity<Modulo>(entity =>
        {
            entity.HasKey(e => e.IdModulo).HasName("PK__Modulo__B2584DFCC84065CB");

            entity.ToTable("Modulo");

            entity.Property(e => e.IdModulo).HasColumnName("id_modulo");
            entity.Property(e => e.NombreModulo)
                .HasMaxLength(50)
                .HasColumnName("nombre_modulo");
        });

        modelBuilder.Entity<ModulosRole>(entity =>
        {
            entity.HasKey(e => e.IdModulosRoles).HasName("PK__ModulosR__25DE0E3656B362BE");

            entity.Property(e => e.IdModulosRoles).HasColumnName("id_modulos_roles");
            entity.Property(e => e.IdModulo).HasColumnName("id_modulo");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");

            entity.HasOne(d => d.IdModuloNavigation).WithMany(p => p.ModulosRoles)
                .HasForeignKey(d => d.IdModulo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModulosRo__id_mo__7F16D496");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.ModulosRoles)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ModulosRo__id_ro__7E22B05D");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.IdNivel).HasName("PK__Nivel__9CAF1C5313F3021C");

            entity.ToTable("Nivel");

            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.NombreNivel)
                .HasMaxLength(50)
                .HasColumnName("Nombre_nivel");
        });

        modelBuilder.Entity<Notum>(entity =>
        {
            entity.HasKey(e => e.IdNota).HasName("PK__Nota__26991D8CABC2BC70");

            entity.Property(e => e.IdNota).HasColumnName("id_nota");
            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Nota__id_alumno__758D6A5C");

            entity.HasOne(d => d.IdBimestreNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdBimestre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Nota__id_bimestr__76818E95");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Nota__id_mesa__74994623");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__6ABCB5E0A15F4B26");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__Sede__D693504BFC184A67");

            entity.ToTable("Sede");

            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreSede)
                .HasMaxLength(50)
                .HasColumnName("nombre_sede");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.IdStaff).HasName("PK__Staff__12FEDA3CCB83381C");

            entity.HasIndex(e => e.NivelAprobado, "IDX_Nivel_aprobado");

            entity.HasIndex(e => e.Dpi, "IDX_Staff_DPI");

            entity.HasIndex(e => e.PrimerApellidoStaff, "IDX_Staff_PrimerApellido");

            entity.HasIndex(e => e.PrimerNombreStaff, "IDX_Staff_PrimerNombre");

            entity.HasIndex(e => e.Dpi, "UQ__Staff__C035891F84D91D7C").IsUnique();

            entity.Property(e => e.IdStaff).HasColumnName("id_staff");
            entity.Property(e => e.ApellidoCasado)
                .HasMaxLength(50)
                .HasColumnName("Apellido_casado");
            entity.Property(e => e.Direccion).HasMaxLength(100);
            entity.Property(e => e.Dpi)
                .HasMaxLength(25)
                .HasColumnName("DPI");
            entity.Property(e => e.EstadoCivil)
                .HasMaxLength(20)
                .HasColumnName("Estado_civil");
            entity.Property(e => e.EstatusStaff).HasColumnName("Estatus_staff");
            entity.Property(e => e.FechaBautizo).HasColumnName("Fecha_bautizo");
            entity.Property(e => e.FechaNacimiento).HasColumnName("Fecha_nacimiento");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.NivelAprobado)
                .HasMaxLength(50)
                .HasColumnName("Nivel_aprobado");
            entity.Property(e => e.NumeroCelula)
                .HasMaxLength(20)
                .HasColumnName("Numero_celula");
            entity.Property(e => e.OtrosNombresStaff)
                .HasMaxLength(50)
                .HasColumnName("Otros_nombres_staff");
            entity.Property(e => e.PrimerApellidoStaff)
                .HasMaxLength(50)
                .HasColumnName("PrimerApellido_staff");
            entity.Property(e => e.PrimerNombreStaff)
                .HasMaxLength(50)
                .HasColumnName("PrimerNombre_staff");
            entity.Property(e => e.SegundoApellidoStaff)
                .HasMaxLength(50)
                .HasColumnName("SegundoApellido_staff");
            entity.Property(e => e.SegundoNombreStaff)
                .HasMaxLength(50)
                .HasColumnName("SegundoNombre_staff");
            entity.Property(e => e.Telefono).HasMaxLength(15);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Staff__id_usuari__52442E1F");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__4E3E04ADCF542AC0");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ContrasenaUsuario)
                .HasMaxLength(50)
                .HasColumnName("contrasena_usuario");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .HasColumnName("Nombre_usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__id_rol__4E739D3B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
