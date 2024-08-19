using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Libro
{
    public int IdLibro { get; set; }

    public string NombreLibro { get; set; }

    public virtual ICollection<Leccion> Leccions { get; set; } = new List<Leccion>();
}
