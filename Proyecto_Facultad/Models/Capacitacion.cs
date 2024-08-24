using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Capacitacion
{
    public int IdCapacitacion { get; set; }

    [DisplayName("Fecha de capacitación")]
    [Required(ErrorMessage = "La fecha de capacitación es obligatoria")]
    public DateOnly FechaCapacitacion { get; set; }

    [DisplayName("Nombre del maestro")]
    
    public int IdStaff { get; set; }
    
    public virtual Staff IdStaffNavigation { get; set; }
    
}
