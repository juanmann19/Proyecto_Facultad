using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Alumno
{
    public int IdAlumno { get; set; }

    public string PrimerNombreAlumno { get; set; } = null!;

    public string? SegundoNombreAlumno { get; set; }

    public string? OtrosNombresAlumno { get; set; }

    public string PrimerApellidoAlumno { get; set; } = null!;

    public string? SegundoApellidoAlumno { get; set; }

    public string? ApellidoCasado { get; set; }

    public DateOnly FechaNacimiento { get; set; }

    public DateOnly? FechaBautizo { get; set; }

    public string Direccion { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? NumeroCelula { get; set; }

    public string? EstadoCivil { get; set; }

    public string? CodigoFrater { get; set; }

    public bool Becado { get; set; }

    public bool Retiro { get; set; }

    public bool ReunionesEnConfianza { get; set; }

    public bool Otros { get; set; }

    public bool EstatusAlumno { get; set; }
}
