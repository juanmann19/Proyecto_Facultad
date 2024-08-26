using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class AsignacionMaestro
{
    public int IdAsignacionmaestros { get; set; }

    public int IdStaff { get; set; }

    public int IdMesa { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; }

    public virtual Staff IdStaffNavigation { get; set; }
}
