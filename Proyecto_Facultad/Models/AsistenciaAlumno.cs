using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Facultad.Models;

public partial class AsistenciaAlumno
{
    public int IdAsistenciaAlumno { get; set; }

    [DisplayName("Nombre Maestro")]
    public int IdAsistenciaStaff { get; set; }

    [DisplayName("Nombre Alumno")]
    public int IdAlumno { get; set; }

    public string Ausencia { get; set; }

    [NotMapped]
    public DateTime FechaAsistencia { get; set; }
    public virtual Alumno IdAlumnoNavigation { get; set; }

    public virtual AsistenciaStaff IdAsistenciaStaffNavigation { get; set; }

}