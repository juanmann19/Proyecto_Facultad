using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class AsignacionAlumno
{
    public int IdAsignacionalumnos { get; set; }

    public int IdAlumno { get; set; }

    public int IdMesa { get; set; }

    public virtual Alumno IdAlumnoNavigation { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; }

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();
}
