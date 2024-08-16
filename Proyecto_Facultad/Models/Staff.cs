using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Staff
{
    public int IdStaff { get; set; }


    [DisplayName("Primer Nombre")]
    [Required(ErrorMessage = "El primer Nombre es Obligatorio")]
    public string PrimerNombreStaff { get; set; } = null!;


    [DisplayName("Segundo Nombre")]

    public string? SegundoNombreStaff { get; set; }


    [DisplayName("Otros Nombres")]
    public string? OtrosNombresStaff { get; set; }

    [DisplayName("Primer Apellido")]
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    public string PrimerApellidoStaff { get; set; } = null!;


    [DisplayName("Segundo Apellido")]
    public string? SegundoApellidoStaff { get; set; }


    [DisplayName("Apellido Casado")]
    public string? ApellidoCasado { get; set; }


    [DisplayName("DPI")]
    public string? Dpi { get; set; }


    [DisplayName("Fecha de Nacimiento")]
    [Required(ErrorMessage = "Ingrese su fecha de nacimiento")]
    public DateOnly FechaNacimiento { get; set; }


    [DisplayName("Fecha de Bautizo")]
    public DateOnly? FechaBautizo { get; set; }


    [DisplayName("Dirección")]
    public string? Direccion { get; set; }


    [DisplayName("Teléfono")]
    public string? Telefono { get; set; }


    [DisplayName("Número de Célula")]
    [Required(ErrorMessage = "Ingrese su numero de celula")]
    public string NumeroCelula { get; set; } = null!;


    [DisplayName("Máximo Nivel Aprobado")]
    [Required(ErrorMessage = "Seleccione el nivel máximo aprobado")]
    public string NivelAprobado { get; set; } = null!;


    [DisplayName("Estado Civil")]
    [Required(ErrorMessage = "Seleccione su estado civil")]
    public string EstadoCivil { get; set; } = null!;


    [DisplayName("Estatus del Staff")]
    [Required(ErrorMessage = "Por favor active su estatus")]
    public bool EstatusStaff { get; set; }


    [DisplayName("Id Usuario")]
    [Required(ErrorMessage = "Ingrese su ID de usuario brindado por el sistema.")]
    public int IdUsuario { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<Capacitacion> Capacitacions { get; set; } = new List<Capacitacion>();

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
