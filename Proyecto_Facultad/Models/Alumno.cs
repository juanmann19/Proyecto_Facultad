using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Alumno
{
    public int IdAlumno { get; set; }


    [DisplayName("Primer Nombre")]
    [Required(ErrorMessage = "El primer nombre es obligatorio")]
    public string PrimerNombreAlumno { get; set; } = null!;
    
    
    
    [DisplayName("Segundo Nombre")]
    public string? SegundoNombreAlumno { get; set; }
    
    
    [DisplayName("Otros Nombres")]
    public string? OtrosNombresAlumno { get; set; }
    
    
    [DisplayName("Primer Apellido")]
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
     public string PrimerApellidoAlumno { get; set; } = null!;


    [DisplayName("Segundo Apellido")]
    public string? SegundoApellidoAlumno { get; set; }



    [DisplayName("Apellido Conyugal")]
    public string? ApellidoCasado { get; set; }


    [DisplayName("Fecha de Nacimiento")]
    //Validacion de rangos de fecha queda pendiente
    //[Range(typeof(DateTime), "01/01/1922", "31/12/2022", ErrorMessage = "La fecha debe estar entre el 1 de enero de 1900 y el 31 de diciembre de 2022.")]

    [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
    public DateOnly FechaNacimiento { get; set; }


    [DisplayName("Fecha de Bautizo")]
    public DateOnly? FechaBautizo { get; set; }


    [DisplayName("Dirección")]
    [Required(ErrorMessage = "El dirección es obligatoria")]
    public string Direccion { get; set; } = null!;


    [DisplayName("Teléfono")]
    public string? Telefono { get; set; }


    [DisplayName("Número de Celula")]
    public string? NumeroCelula { get; set; }


    [DisplayName("Estado Civil")]
    public string? EstadoCivil { get; set; }


    [DisplayName("Código Frater")]
    public string? CodigoFrater { get; set; }


    [DisplayName("Becado")]
    public bool Becado { get; set; }


    [DisplayName("Retiro")]
    public bool Retiro { get; set; }


    [DisplayName("Reuniones en Confianza")]
    public bool ReunionesEnConfianza { get; set; }


    [DisplayName("Otros")]
    public bool Otros { get; set; }


    [DisplayName("Estatus Alumno")]
    public bool EstatusAlumno { get; set; }
}
