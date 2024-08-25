using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Nota
{
    public int IdNota { get; set; }

    public int IdAsignacionalumnos { get; set; }

    public int IdBimestre { get; set; }

    public int NotaObtenida { get; set; }

    public virtual AsignacionAlumno IdAsignacionalumnosNavigation { get; set; }

    public virtual Bimestre IdBimestreNavigation { get; set; }
}
