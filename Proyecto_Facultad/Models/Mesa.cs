using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Mesa
{
    public int IdMesa { get; set; }

    public int IdSede { get; set; }

    public int IdJornada { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public bool EstadoMesa { get; set; }

    public virtual ICollection<AsignacionAlumno> AsignacionAlumnos { get; set; } = new List<AsignacionAlumno>();

    public virtual ICollection<AsignacionMaestro> AsignacionMaestros { get; set; } = new List<AsignacionMaestro>();

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();

    public virtual Jornadum IdJornadaNavigation { get; set; }

    public virtual Sede IdSedeNavigation { get; set; }
}
