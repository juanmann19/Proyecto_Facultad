using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Leccion
{
    public int IdLeccion { get; set; }

    public string NombreLeccion { get; set; } = null!;

    public int IdLibro { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual Libro IdLibroNavigation { get; set; } = null!;
}
