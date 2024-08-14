using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Bimestre
{
    public int IdBimestre { get; set; }

    public string? NombreBimestre { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
