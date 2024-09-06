using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Alumno
{
    public int IdAlumno { get; set; }

    [DisplayName("Primer Nombre")]
    public string PrimerNombreAlumno { get; set; }

    [DisplayName("Segundo Nombre")]
    public string SegundoNombreAlumno { get; set; }

    [DisplayName("Otros Nombres")]
    public string OtrosNombresAlumno { get; set; }

    [DisplayName("Primer Apellido")]
    public string PrimerApellidoAlumno { get; set; }

    [DisplayName("Segundo Apellido")]
    public string SegundoApellidoAlumno { get; set; }

    [DisplayName("Apellido de Casado")]
    public string ApellidoCasado { get; set; }

    [DisplayName("Fecha de Nacimiento")]
    public DateOnly FechaNacimientoAlumno { get; set; }

    [DisplayName("Fecha de Bautizo del Alumno")]
    public DateOnly? FechaBautizoAlumno { get; set; }

    public string Direccion { get; set; }

    public string Telefono { get; set; }

    [DisplayName("DPI")]
    public string Dpi { get; set; }

    [DisplayName("NIT")]
    public string Nit { get; set; }

    [DisplayName("Estado Civil")]
    public string EstadoCivil { get; set; }

    public string Genero { get; set; }

    [DisplayName("Numero de Celula")]
    public string NumeroCelula { get; set; }

    [DisplayName("Codigo Frater")]
    public string CodigoFrater { get; set; }

    [DisplayName("Inscrito")]
    public bool Becado { get; set; } 

    public bool Retiro { get; set; }

    public bool ReunionesEnConfianza { get; set; }

    public bool Otros { get; set; }

    public bool EstatusAlumno { get; set; }

    public virtual ICollection<AsignacionAlumno> AsignacionAlumnos { get; set; } = new List<AsignacionAlumno>();

    public virtual ICollection<AsistenciaAlumno> AsistenciaAlumnos { get; set; } = new List<AsistenciaAlumno>();
}
