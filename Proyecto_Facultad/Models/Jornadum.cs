using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Jornadum
{
    public int IdJornada { get; set; }

    [DisplayName("Dia de la Jornada")]
    public string DiaSemana { get; set; }

    [DisplayName("Horario de la Jornada")]
    public string Horario { get; set; }


    [DisplayName("Sede")]
    public int IdSede { get; set; }

    public virtual Sede IdSedeNavigation { get; set; }

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
