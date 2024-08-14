using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Notum
{
    public int IdNota { get; set; }

    public int? IdMesa { get; set; }

    public int? IdAlumno { get; set; }

    public int? IdBimestre { get; set; }

    public virtual Alumno? IdAlumnoNavigation { get; set; }

    public virtual Bimestre? IdBimestreNavigation { get; set; }

    public virtual Mesa? IdMesaNavigation { get; set; }
}
