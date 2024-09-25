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

    public virtual DbSet<AsignacionAlumno> AsignacionAlumnos { get; set; }

    public virtual DbSet<AsignacionMaestro> AsignacionMaestros { get; set; }

    public virtual DbSet<AsistenciaAlumno> AsistenciaAlumnos { get; set; }

    public virtual DbSet<AsistenciaStaff> AsistenciaStaffs { get; set; }

    public virtual DbSet<AvanceMesa> AvanceMesas { get; set; }

    public virtual DbSet<Bimestre> Bimestres { get; set; }

    public virtual DbSet<Capacitacion> Capacitacions { get; set; }

    public virtual DbSet<Jornadum> Jornada { get; set; }

    public virtual DbSet<Leccion> Leccions { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<Mesa> Mesas { get; set; }

    public virtual DbSet<Nivel> Nivels { get; set; }

    public virtual DbSet<Nota> Notas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }




    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.IdAlumno).HasName("PK__Alumno__6D77A7F182067B85");

            entity.ToTable("Alumno");

            entity.HasIndex(e => e.CodigoFrater, "IDX_Alumno_CodigoFrater");

            entity.HasIndex(e => e.SegundoApellidoAlumno, "IDX_Alumno_PrimerApellido");

            entity.HasIndex(e => e.PrimerNombreAlumno, "IDX_Alumno_PrimerNombre");

            entity.HasIndex(e => e.CodigoFrater, "IDX_Staff_CodigoFrater");

            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.ApellidoCasado)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Apellido_casado");
            entity.Property(e => e.Becado).HasColumnName("becado");
            entity.Property(e => e.CodigoFrater)
                .HasMaxLength(20)
                .HasColumnName("Codigo_Frater");
            entity.Property(e => e.Direccion)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dpi)
                .HasMaxLength(15)
                .HasColumnName("DPI");
            entity.Property(e => e.EstadoCivil)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("Estado_civil");
            entity.Property(e => e.EstatusAlumno)
                .HasDefaultValue(true)
                .HasColumnName("Estatus_alumno");
            entity.Property(e => e.FechaBautizoAlumno).HasColumnName("Fecha_bautizo_alumno");
            entity.Property(e => e.FechaNacimientoAlumno).HasColumnName("Fecha_nacimiento_alumno");
            entity.Property(e => e.Genero)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.Nit)
                .HasMaxLength(15)
                .HasColumnName("NIT");
            entity.Property(e => e.NumeroCelula)
                .HasMaxLength(20)
                .HasColumnName("Numero_celula");
            entity.Property(e => e.OtrosNombresAlumno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Otros_nombres_alumno");
            entity.Property(e => e.PrimerApellidoAlumno)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("PrimerApellido_alumno");
            entity.Property(e => e.PrimerNombreAlumno)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("PrimerNombre_alumno");
            entity.Property(e => e.SegundoApellidoAlumno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("SegundoApellido_alumno");
            entity.Property(e => e.SegundoNombreAlumno)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("SegundoNombre_alumno");
            entity.Property(e => e.Telefono).HasMaxLength(15);
        });

        modelBuilder.Entity<AsignacionAlumno>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionalumnos).HasName("PK__Asignaci__7EB2BA60EE7B8BB9");

            entity.Property(e => e.IdAsignacionalumnos).HasColumnName("id_asignacionalumnos");
            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.AsignacionAlumnos)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asignacio__id_al__1EA48E88");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.AsignacionAlumnos)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asignacio__id_me__1F98B2C1");
        });

        modelBuilder.Entity<AsignacionMaestro>(entity =>
        {
            entity.HasKey(e => e.IdAsignacionmaestros).HasName("PK__Asignaci__8E62ABC7B9E7BB81");

            entity.Property(e => e.IdAsignacionmaestros).HasColumnName("id_asignacionmaestros");
            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.AsignacionMaestros)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asignacio__id_me__37703C52");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.AsignacionMaestros)
                .HasForeignKey(d => d.IdStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asignacio__id_st__367C1819");
        });

        modelBuilder.Entity<AsistenciaAlumno>(entity =>
        {
            entity.HasKey(e => e.IdAsistenciaAlumno).HasName("PK__Asistenc__0FD56E6D69E51D4E");

            entity.ToTable("AsistenciaAlumno");

            entity.Property(e => e.IdAsistenciaAlumno)
                .ValueGeneratedNever()
                .HasColumnName("id_asistencia_alumno");
            entity.Property(e => e.Ausencia)
                .HasMaxLength(50)
                .HasColumnName("ausencia");
            entity.Property(e => e.IdAlumno).HasColumnName("id_alumno");
            entity.Property(e => e.IdAsistenciaStaff).HasColumnName("id_asistencia_staff");

            entity.HasOne(d => d.IdAlumnoNavigation).WithMany(p => p.AsistenciaAlumnos)
                .HasForeignKey(d => d.IdAlumno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_al__40058253");

            entity.HasOne(d => d.IdAsistenciaStaffNavigation).WithMany(p => p.AsistenciaAlumnos)
                .HasForeignKey(d => d.IdAsistenciaStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_as__40F9A68C");
        });

        modelBuilder.Entity<AsistenciaStaff>(entity =>
        {
            entity.HasKey(e => e.IdAsistenciaStaff).HasName("PK__Asistenc__4E497B3560D80BC0");

            entity.ToTable("AsistenciaStaff");

            entity.HasIndex(e => e.IdBimestre, "IX_AsistenciaStaff_Bimestre");

            entity.HasIndex(e => e.IdLeccion, "IX_AsistenciaStaff_Leccion");

            entity.HasIndex(e => e.IdStaff, "IX_AsistenciaStaff_Staff");

            entity.Property(e => e.IdAsistenciaStaff)
                .ValueGeneratedNever()
                .HasColumnName("id_asistencia_staff");
            entity.Property(e => e.Ausencia)
                .HasMaxLength(50)
                .HasColumnName("ausencia");
            entity.Property(e => e.FechaClase).HasColumnName("fecha_clase");
            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.IdLeccion).HasColumnName("id_leccion");
            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");

            entity.HasOne(d => d.IdBimestreNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdBimestre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_bi__3D2915A8");

            entity.HasOne(d => d.IdLeccionNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdLeccion)
                .HasConstraintName("FK__Asistenci__id_le__3C34F16F");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_me__3B40CD36");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.AsistenciaStaffs)
                .HasForeignKey(d => d.IdStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Asistenci__id_st__3A4CA8FD");
        });

        modelBuilder.Entity<AvanceMesa>(entity =>
        {
            entity.HasKey(e => e.IdAvancemesa).HasName("PK__AvanceMe__8E5E875A30A4AC5F");

            entity.ToTable("AvanceMesa");

            entity.Property(e => e.IdAvancemesa).HasColumnName("id_avancemesa");
            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.IdLeccion).HasColumnName("id_leccion");
            entity.Property(e => e.IdLibro).HasColumnName("id_libro");
            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");
            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");

            entity.HasOne(d => d.IdBimestreNavigation).WithMany(p => p.AvanceMesas)
                .HasForeignKey(d => d.IdBimestre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AvanceMes__id_bi__236943A5");

            entity.HasOne(d => d.IdLeccionNavigation).WithMany(p => p.AvanceMesas)
                .HasForeignKey(d => d.IdLeccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AvanceMes__id_le__25518C17");

            entity.HasOne(d => d.IdLibroNavigation).WithMany(p => p.AvanceMesas)
                .HasForeignKey(d => d.IdLibro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AvanceMes__id_li__245D67DE");

            entity.HasOne(d => d.IdMesaNavigation).WithMany(p => p.AvanceMesas)
                .HasForeignKey(d => d.IdMesa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AvanceMes__id_me__2645B050");

            entity.HasOne(d => d.IdNivelNavigation).WithMany(p => p.AvanceMesas)
                .HasForeignKey(d => d.IdNivel)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AvanceMes__id_ni__22751F6C");
        });

        modelBuilder.Entity<Bimestre>(entity =>
        {
            entity.HasKey(e => e.IdBimestre).HasName("PK__Bimestre__7F1985655CDDCED9");

            entity.ToTable("Bimestre");

            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.NombreBimestre)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre_bimestre");
        });

        modelBuilder.Entity<Capacitacion>(entity =>
        {
            entity.HasKey(e => e.IdCapacitacion).HasName("PK__Capacita__FA471D9B0657B481");

            entity.ToTable("Capacitacion");

            entity.Property(e => e.IdCapacitacion).HasColumnName("id_capacitacion");
            entity.Property(e => e.FechaCapacitacion).HasColumnName("fecha_capacitacion");
            entity.Property(e => e.IdStaff).HasColumnName("id_staff");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.Capacitacions)
                .HasForeignKey(d => d.IdStaff)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Capacitac__id_st__43D61337");
        });

        modelBuilder.Entity<Jornadum>(entity =>
        {
            entity.HasKey(e => e.IdJornada).HasName("PK__Jornada__6BD46D1A55E1D257");

            entity.Property(e => e.IdJornada).HasColumnName("id_jornada");
            entity.Property(e => e.DiaSemana)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("dia_semana");
            entity.Property(e => e.Horario)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnName("horario");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Jornada)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Jornada__id_sede__10566F31");
        });

        modelBuilder.Entity<Leccion>(entity =>
        {
            entity.HasKey(e => e.IdLeccion).HasName("PK__Leccion__2015E24C54B9FF41");

            entity.ToTable("Leccion");

            entity.Property(e => e.IdLeccion).HasColumnName("id_leccion");
            entity.Property(e => e.Descripcion)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdLibro).HasColumnName("id_libro");

            entity.HasOne(d => d.IdLibroNavigation).WithMany(p => p.Leccions)
                .HasForeignKey(d => d.IdLibro)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Leccion__id_libr__17036CC0");
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.HasKey(e => e.IdLibro).HasName("PK__Libro__EC09C24EE6929BD1");

            entity.ToTable("Libro");

            entity.Property(e => e.IdLibro).HasColumnName("id_libro");
            entity.Property(e => e.NombreLibro)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre_libro");
        });

        modelBuilder.Entity<Mesa>(entity =>
        {
            entity.HasKey(e => e.IdMesa).HasName("PK__Mesa__68A1E15914A14284");

            entity.ToTable("Mesa");

            entity.HasIndex(e => e.IdJornada, "IX_Mesa_Jornada");

            entity.HasIndex(e => e.IdSede, "IX_Mesa_Sede");

            entity.Property(e => e.IdMesa).HasColumnName("id_mesa");
            entity.Property(e => e.EstadoMesa)
                .HasDefaultValue(true)
                .HasColumnName("Estado_mesa");
            entity.Property(e => e.FechaFin).HasColumnName("Fecha_fin");
            entity.Property(e => e.FechaInicio).HasColumnName("Fecha_inicio");
            entity.Property(e => e.IdJornada).HasColumnName("id_jornada");
            entity.Property(e => e.IdSede).HasColumnName("id_sede");

            entity.HasOne(d => d.IdJornadaNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdJornada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mesa__id_jornada__1BC821DD");

            entity.HasOne(d => d.NombreSedeNavigation).WithMany(p => p.Mesas)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Mesa__id_sede__1AD3FDA4");
        });

        modelBuilder.Entity<Nivel>(entity =>
        {
            entity.HasKey(e => e.IdNivel).HasName("PK__Nivel__9CAF1C532F5C60CE");

            entity.ToTable("Nivel");

            entity.Property(e => e.IdNivel).HasColumnName("id_nivel");
            entity.Property(e => e.NombreNivel)
                .IsRequired()
                .HasMaxLength(25)
                .HasColumnName("Nombre_nivel");
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.HasKey(e => e.IdNota).HasName("PK__Notas__26991D8CA0FB796E");

            entity.Property(e => e.IdNota).HasColumnName("id_nota");
            entity.Property(e => e.IdAsignacionalumnos).HasColumnName("id_asignacionalumnos");
            entity.Property(e => e.IdBimestre).HasColumnName("id_bimestre");
            entity.Property(e => e.NotaObtenida).HasColumnName("nota_obtenida");

            entity.HasOne(d => d.IdAsignacionalumnosNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdAsignacionalumnos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notas__id_asigna__29221CFB");

            entity.HasOne(d => d.IdBimestreNavigation).WithMany(p => p.Nota)
                .HasForeignKey(d => d.IdBimestre)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notas__id_bimest__2A164134");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__6ABCB5E0E5CFF244");

            entity.ToTable("Rol");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__Sede__D693504B6566EBCF");

            entity.ToTable("Sede");

            entity.Property(e => e.IdSede).HasColumnName("id_sede");
            entity.Property(e => e.NombreSede)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("nombre_sede");
            entity.Property(e => e.Pais)
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("pais");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.IdStaff).HasName("PK__Staff__12FEDA3CD0D1A5F9");

            entity.HasIndex(e => e.SegundoApellidoStaff, "IDX_Staff_PrimerApellido");

            entity.HasIndex(e => e.PrimerNombreStaff, "IDX_Staff_PrimerNombre");

            entity.HasIndex(e => e.IdUsuario, "IX_Staff_Usuario");

            entity.Property(e => e.IdStaff).HasColumnName("id_staff");
            entity.Property(e => e.ApellidoCasado)
                .HasMaxLength(50)
                .HasColumnName("Apellido_casado");
            entity.Property(e => e.CodigoFrater)
                .HasMaxLength(20)
                .HasColumnName("Codigo_Frater");
            entity.Property(e => e.Direccion).HasMaxLength(100);
            entity.Property(e => e.Dpi)
                .HasMaxLength(15)
                .HasColumnName("DPI");
            entity.Property(e => e.EstadoCivil)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("Estado_civil");
            entity.Property(e => e.EstatusStaff)
                .HasDefaultValue(true)
                .HasColumnName("Estatus_staff");
            entity.Property(e => e.FechaBautizoStaff).HasColumnName("Fecha_bautizo_staff");
            entity.Property(e => e.FechaNacimientoStaff).HasColumnName("Fecha_nacimiento_staff");
            entity.Property(e => e.Genero)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Nit)
                .HasMaxLength(15)
                .HasColumnName("NIT");
            entity.Property(e => e.NivelAprobado)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Nivel_aprobado");
            entity.Property(e => e.NumeroCelula)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("Numero_celula");
            entity.Property(e => e.OtrosNombresStaff)
                .HasMaxLength(50)
                .HasColumnName("Otros_nombres_staff");
            entity.Property(e => e.PrimerApellidoStaff)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PrimerApellido_staff");
            entity.Property(e => e.PrimerNombreStaff)
                .IsRequired()
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
                .HasConstraintName("FK__Staff__id_usuari__339FAB6E");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__4E3E04ADF69494AD");

            entity.HasIndex(e => e.IdRol, "IX_Usuarios_Rol");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.ContrasenaUsuario)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("contrasena_usuario");
            entity.Property(e => e.EstadoUsuario)
                .HasDefaultValue(true)
                .HasColumnName("Estado_usuario");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreUsuario)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Nombre_usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuarios__id_rol__2FCF1A8A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
