using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Facultad.Models;

public partial class AsistenciaStaff
{
    [Key] // Asegúrate de tener este atributo
    public int IdAsistenciaStaff { get; set; }

    [DisplayName("Nombre")]
    public int IdStaff { get; set; }

    [DisplayName("Fecha de clases")]
    public DateOnly FechaClase { get; set; }

    [DisplayName("Numero de Mesa")]
    public int IdMesa { get; set; }

    [DisplayName("Leccion")]
    public int? IdLeccion { get; set; }

    [DisplayName("Bimestre")]
    public int IdBimestre { get; set; }

    public string Ausencia { get; set; }

    public virtual ICollection<AsistenciaAlumno> AsistenciaAlumnos { get; set; } = new List<AsistenciaAlumno>();

    public virtual Bimestre IdBimestreNavigation { get; set; }

    public virtual Leccion IdLeccionNavigation { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; }

    public virtual Staff IdStaffNavigation { get; set; }
}
