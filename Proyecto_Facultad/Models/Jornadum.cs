using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Jornadum
{
    [DisplayName("Id Jornada")]
    public int IdJornada { get; set; }

    [DisplayName("Dia Jornada")]
    public string DiaJornada { get; set; }

    [DisplayName("Horario Jornada")]
    public string HorarioJornada { get; set; }

    [DisplayName("Id Sede")]
    public int IdSede { get; set; }

    public virtual Sede IdSedeNavigation { get; set; }

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
