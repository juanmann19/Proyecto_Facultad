using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Staff
{
    public class FechaNoFuturaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateOnly fecha)
            {
                return fecha <= DateOnly.FromDateTime(DateTime.Now);
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"La fecha {name} no puede ser mayor que la fecha actual.";
        }
    }
    public int IdStaff { get; set; }


    [DisplayName("Primer Nombre")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    [Required(ErrorMessage = "El primer Nombre es Obligatorio")]
    public string PrimerNombreStaff { get; set; } = null!;


    [DisplayName("Segundo Nombre")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]

    public string? SegundoNombreStaff { get; set; }


    [DisplayName("Otros Nombres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ\s]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string? OtrosNombresStaff { get; set; }

    [DisplayName("Primer Apellido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    public string PrimerApellidoStaff { get; set; } = null!;


    [DisplayName("Segundo Apellido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string? SegundoApellidoStaff { get; set; }


    [DisplayName("Apellido Casado")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string? ApellidoCasado { get; set; }


    [DisplayName("DPI")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "El DPI debe tener 13 dígitos.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El teléfono debe contener exactamente 13 dígitos sin guiones.")]
    public string? Dpi { get; set; }


    [DisplayName("Fecha de Nacimiento")]
    [Required(ErrorMessage = "Ingrese su fecha de nacimiento")]
    [FechaNoFutura(ErrorMessage = "La fecha de nacimiento no puede ser mayor que la fecha actual.")]

    public DateOnly FechaNacimiento { get; set; }


    [DisplayName("Fecha de Bautizo")]
    [FechaNoFutura(ErrorMessage = "La fecha de nacimiento no puede ser mayor que la fecha actual.")]

    public DateOnly? FechaBautizo { get; set; }


    [DisplayName("Dirección")]
    public string? Direccion { get; set; }


    [DisplayName("Teléfono")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El teléfono debe tener 8 dígitos.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe contener exactamente 8 dígitos sin guiones.")]

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
