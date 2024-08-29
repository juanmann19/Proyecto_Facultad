using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class AsistenciaAlumno
{
    public int IdAsistenciaAlumno { get; set; }

    public int IdAsistenciaStaff { get; set; }

    public int IdAlumno { get; set; }

    public string Ausencia { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; }

    public virtual AsistenciaStaff IdAsistenciaStaffNavigation { get; set; }
}
