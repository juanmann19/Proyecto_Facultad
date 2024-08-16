using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Mesa
{
    public int IdMesa { get; set; }

    public DateOnly FechaInicio { get; set; }

    public int IdStaff { get; set; }

    public int IdAlumno { get; set; }

    public int IdJornada { get; set; }

    public int IdNivel { get; set; }

    public int IdHorario { get; set; }

    public int AnioAsignacion { get; set; }

    public bool EstadoMesa { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual Horario IdHorarioNavigation { get; set; } = null!;

    public virtual Jornadum IdJornadaNavigation { get; set; } = null!;

    public virtual Nivel IdNivelNavigation { get; set; } = null!;

    public virtual Staff IdStaffNavigation { get; set; } = null!;

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
