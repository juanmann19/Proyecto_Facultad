using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Jornadum
{
    public int IdJornada { get; set; }

    public string DiaJornada { get; set; } = null!;

    public string HorarioJornada { get; set; } = null!;

    public int IdSede { get; set; }

    public virtual Sede IdSedeNavigation { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
