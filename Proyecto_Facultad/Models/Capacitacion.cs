using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Capacitacion
{
    public int IdCapacitacion { get; set; }

    [DisplayName("Fecha de Capacitacion")]
    public DateTime FechaCapacitacion { get; set; }

    [DisplayName("Nombre del Maestro")]
    public int IdStaff { get; set; }

    public virtual Staff IdStaffNavigation { get; set; }
}
