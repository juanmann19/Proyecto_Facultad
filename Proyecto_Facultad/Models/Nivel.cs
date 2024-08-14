using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }

    public string? NombreNivel { get; set; }

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
