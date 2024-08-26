using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Jornadum
{
    public int IdJornada { get; set; }

    public int DiaSemana { get; set; }

    public int Horario { get; set; }

    public int IdSede { get; set; }

    public virtual Sede IdSedeNavigation { get; set; }

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
