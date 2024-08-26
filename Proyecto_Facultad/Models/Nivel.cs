using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }

    public string NombreNivel { get; set; }

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();
}
