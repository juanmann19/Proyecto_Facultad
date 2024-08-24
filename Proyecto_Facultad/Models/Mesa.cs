using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Mesa
{
    public int IdMesa { get; set; }
    
    [DisplayName("Fecha Inicio")]
    [Required(ErrorMessage = "La Fecha inicio es Obligatoria")]
    public DateOnly FechaInicio { get; set; }

    [DisplayName("Staff")]
    [Required(ErrorMessage = "El Staff  es Obligatorio")]
    public int IdStaff { get; set; }

    [DisplayName("Alumno")]
    [Required(ErrorMessage = "El Alumno es Obligatorio")]
    public int IdAlumno { get; set; }

    [DisplayName("Jornada")]
    [Required(ErrorMessage = "La Jornada es Obligatoria")]
    public int IdJornada { get; set; }

    [DisplayName("Nivel")]
    [Required(ErrorMessage = "El Nivel es Obligatorio")]
    public int IdNivel { get; set; }

    [DisplayName("Año Asignación")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Campo puede contener numeros")]
    [Required(ErrorMessage = "El año asignacion es Obligatorio")]
    public int AnioAsignacion { get; set; }

    [DisplayName("Estado Mesa")]
    [Required(ErrorMessage = "El estado es Obligatorio")]
    public bool EstadoMesa { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual Alumno IdAlumnoNavigation { get; set; }

    public virtual Jornadum IdJornadaNavigation { get; set; }

    public virtual Nivel IdNivelNavigation { get; set; }

    public virtual Staff IdStaffNavigation { get; set; }

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
