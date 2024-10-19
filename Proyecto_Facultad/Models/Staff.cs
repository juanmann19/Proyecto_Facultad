using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Proyecto_Facultad.Models.Alumno;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Facultad.Models;

public partial class Staff
{
    public int IdStaff { get; set; }

    [DisplayName("Primer nombre")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    [Required(ErrorMessage = "El primer Nombre es Obligatorio")]
    public string PrimerNombreStaff { get; set; }



    [DisplayName("Segundo nombre")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string SegundoNombreStaff { get; set; }

    [DisplayName("Otros nombres")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ\s]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string OtrosNombresStaff { get; set; }


    [DisplayName("Primer apellido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    [Required(ErrorMessage = "El primer apellido es obligatorio")]
    public string PrimerApellidoStaff { get; set; }


    [DisplayName("Segundo apellido")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string SegundoApellidoStaff { get; set; }


    [DisplayName("Apellido casado")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string ApellidoCasado { get; set; }


    [DisplayName("Fecha de nacimiento")]
    [Required(ErrorMessage = "Ingrese su fecha de nacimiento")]
    [FechaNoFutura(ErrorMessage = "La fecha de nacimiento no puede ser mayor que la fecha actual.")]
    public DateOnly FechaNacimientoStaff { get; set; }



    [DisplayName("Fecha de bautizo")]
    [FechaNoFutura(ErrorMessage = "La fecha de nacimiento no puede ser mayor que la fecha actual.")]
    public DateOnly? FechaBautizoStaff { get; set; }

    [DisplayName("Dirección")]
    public string Direccion { get; set; }


    [DisplayName("Teléfono")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El telefono debe tener 8 digitos")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El telefono debe contener exactamente 8 dígitos sin guiones.")]
    public string Telefono { get; set; }

    [DisplayName("DPI")]
    [StringLength(13, MinimumLength = 13, ErrorMessage = "El DPI debe tener 13 dígitos.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El DPI debe contener exactamente 13 dígitos sin guiones.")]
    public string Dpi { get; set; }

    [DisplayName("NIT")]
    public string Nit { get; set; }

    [DisplayName("Estado civil")]
    public string EstadoCivil { get; set; }


    [Required(ErrorMessage = "Ingrese su género")]

    public string Genero { get; set; }

    [DisplayName("Pertenece a la Red")]
    [Required(ErrorMessage = "Debe seleccionar una red")] 
    public string NumeroCelula { get; set; }

    [DisplayName("Nivel aprobado")]
    [Required(ErrorMessage = "Debe seleccionar un nivel")]


    public string NivelAprobado { get; set; }

    [DisplayName("Codigo frater")]
    public string CodigoFrater { get; set; }

    [DisplayName("Estatus")]
    public bool EstatusStaff { get; set; }

    [DisplayName("Usuario")]
    [Required(ErrorMessage = "Debe seleccionar un usuario")]

    public int IdUsuario { get; set; }

   
    public virtual ICollection<AsignacionMaestro> AsignacionMaestros { get; set; } = new List<AsignacionMaestro>();

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<Capacitacion> Capacitacions { get; set; } = new List<Capacitacion>();

    public virtual Usuario IdUsuarioNavigation { get; set; }

}
