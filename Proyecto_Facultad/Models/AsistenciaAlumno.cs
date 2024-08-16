using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class AsistenciaAlumno
{
    public int IdAsistencia { get; set; }

    public int IdAlumno { get; set; }

    public int IdAsistenciaStaff { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; } = null!;

    public virtual AsistenciaStaff IdAsistenciaStaffNavigation { get; set; } = null!;
}
