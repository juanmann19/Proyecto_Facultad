using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class AvanceMesa
{
    public int IdAvancemesa { get; set; }

    public int IdNivel { get; set; }

    public int IdBimestre { get; set; }

    public int IdLibro { get; set; }

    public int IdLeccion { get; set; }

    public int IdMesa { get; set; }

    public virtual Bimestre IdBimestreNavigation { get; set; }

    public virtual Leccion IdLeccionNavigation { get; set; }

    public virtual Libro IdLibroNavigation { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; }

    public virtual Nivel IdNivelNavigation { get; set; }
}
