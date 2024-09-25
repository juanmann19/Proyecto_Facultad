using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Sede
{
    public int IdSede { get; set; }

    [Required]
    [Display(Name = "Nombre de la sede")]
    public string NombreSede { get; set; }

    public string Pais { get; set; }
    //public virtual Sede NombreSedeNavigation { get; set; }
    public virtual ICollection<Jornadum> Jornada { get; set; } = new List<Jornadum>();

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
