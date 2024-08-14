using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public string? NombreHorario { get; set; }

    public string? DiaHorario { get; set; }

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
