using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Horario
{
    public int IdHorario { get; set; }

    public string NombreHorario { get; set; } = null!;

    public string DiaHorario { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
