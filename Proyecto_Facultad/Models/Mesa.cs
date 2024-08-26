using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Mesa
{
  
    public int IdMesa { get; set; }

    [DisplayName("Fecha de inicio")]
    [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
    public DateOnly FechaInicio { get; set; }

    [DisplayName("Maestro asignado")]
    public int IdStaff { get; set; }

    [DisplayName("Alumno asignado")]
    public int IdAlumno { get; set; }

    [DisplayName("Jornada asignada")]
    public int IdJornada { get; set; }

    [DisplayName("Nivel asignado")]
    public int IdNivel { get; set; }

    [DisplayName("Año de asignación")]
    public int AnioAsignacion { get; set; }

    [DisplayName("Estado de la mesa")]
    public bool EstadoMesa { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    [DisplayName("Alumno asignado")]
    public virtual Alumno IdAlumnoNavigation { get; set; }

    public virtual Jornadum IdJornadaNavigation { get; set; }

    public virtual Nivel IdNivelNavigation { get; set; }

    public virtual Staff IdStaffNavigation { get; set; }

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
