using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }


    [DisplayName("Nombre del nivel")]
    public string NombreNivel { get; set; }

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();
}
