using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Mesa
{
    public int IdMesa { get; set; }

    public int IdSede { get; set; }
    [DisplayName("Fecha de inicio")]
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public DateOnly FechaInicio { get; set; }

    public int IdJornada { get; set; }
    [DisplayName("Maestro asignado")]
    public int IdStaff { get; set; }

    public DateOnly FechaInicio { get; set; }
    [DisplayName("Alumno asignado")]
    public int IdAlumno { get; set; }

    public DateOnly? FechaFin { get; set; }
    [DisplayName("Jornada asignada")]
    public int IdJornada { get; set; }

    public bool EstadoMesa { get; set; }
    [DisplayName("Nivel asignado")]
    public int IdNivel { get; set; }

    public virtual ICollection<AsignacionAlumno> AsignacionAlumnos { get; set; } = new List<AsignacionAlumno>();
    [DisplayName("Año de asignación")]
    public int AnioAsignacion { get; set; }

    public virtual ICollection<AsignacionMaestro> AsignacionMaestros { get; set; } = new List<AsignacionMaestro>();
    [DisplayName("Estado de la mesa")]
    public bool EstadoMesa { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();
    [DisplayName("Alumno asignado")]
    public virtual Alumno IdAlumnoNavigation { get; set; }

    public virtual Jornadum IdJornadaNavigation { get; set; }

    public virtual Sede IdSedeNavigation { get; set; }
}
