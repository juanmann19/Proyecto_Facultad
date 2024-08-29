using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Bimestre
{
    public int IdBimestre { get; set; }

    public string NombreBimestre { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();
}
