using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Mesa
{
    [DisplayName ("Mesa")]
    public int IdMesa { get; set; }

    [DisplayName("Nombre Sede")]
    public int IdSede { get; set; }

    [DisplayName("Jornada")]
    public int IdJornada { get; set; }

    [DisplayName("Fecha de Inicio")]
    public DateOnly FechaInicio { get; set; }

    [DisplayName("Fecha Fin")]
    public DateOnly? FechaFin { get; set; }

    public bool EstadoMesa { get; set; }

    public virtual ICollection<AsignacionAlumno> AsignacionAlumnos { get; set; } = new List<AsignacionAlumno>();

    public virtual ICollection<AsignacionMaestro> AsignacionMaestros { get; set; } = new List<AsignacionMaestro>();

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();

    public virtual Jornadum IdJornadaNavigation { get; set; }

    public virtual Sede NombreSedeNavigation { get; set; }
}
