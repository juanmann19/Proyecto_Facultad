using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Capacitacion
{
    public int IdCapacitacion { get; set; }

    public DateOnly? FechaCapacitacion { get; set; }

    public int? IdStaff { get; set; }

    public virtual Staff? IdStaffNavigation { get; set; }
}
