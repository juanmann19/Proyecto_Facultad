using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Leccion
{
    public int IdLeccion { get; set; }

    public string Descripcion { get; set; }

    public int IdLibro { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();

    public virtual Libro IdLibroNavigation { get; set; }
}
