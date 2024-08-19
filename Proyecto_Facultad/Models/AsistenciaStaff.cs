using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class AsistenciaStaff
{
    public int IdAsistenciaStaff { get; set; }

    public int IdStaff { get; set; }

    public int IdLeccion { get; set; }

    public DateOnly FechaClase { get; set; }

    public int IdBimestre { get; set; }

    public int IdMesa { get; set; }

    public virtual ICollection<AsistenciaAlumno> AsistenciaAlumnos { get; set; } = new List<AsistenciaAlumno>();

    public virtual Bimestre IdBimestreNavigation { get; set; }

    public virtual Leccion IdLeccionNavigation { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; }

    public virtual Staff IdStaffNavigation { get; set; }
}
