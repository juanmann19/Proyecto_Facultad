using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Jornadum
{
    public int IdJornada { get; set; }

    public string DiaJornada { get; set; }

    public string HorarioJornada { get; set; }

    public int IdSede { get; set; }

    public virtual Sede IdSedeNavigation { get; set; }

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
