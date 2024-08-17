using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models
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

    public partial class Alumno
    {
        public int IdAlumno { get; set; }

        [DisplayName("Primer Nombre")]
        [Required(ErrorMessage = "El primer nombre es obligatorio")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "El primer nombre solo puede contener letras")]
        
        public string PrimerNombreAlumno { get; set; } = null!;

        [DisplayName("Segundo Nombre")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "El primer nombre solo puede contener letras")]
        public string? SegundoNombreAlumno { get; set; }

        [DisplayName("Otros Nombres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ\s]+$", ErrorMessage = "El primer nombre solo puede contener letras")]
        public string? OtrosNombresAlumno { get; set; }

        [DisplayName("Primer Apellido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "El primer nombre solo puede contener letras")]
        [Required(ErrorMessage = "El primer apellido es obligatorio")]
        public string PrimerApellidoAlumno { get; set; } = null!;

        [DisplayName("Segundo Apellido")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "El primer nombre solo puede contener letras")]
        public string? SegundoApellidoAlumno { get; set; }

        [DisplayName("Apellido Conyugal")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ]+$", ErrorMessage = "El primer nombre solo puede contener letras")]
        public string? ApellidoCasado { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
        [FechaNoFutura(ErrorMessage = "La fecha de nacimiento no puede ser mayor que la fecha actual.")]
        public DateOnly FechaNacimiento { get; set; }

        [DisplayName("Fecha de Bautizo")]
        [FechaNoFutura(ErrorMessage = "La fecha de bautizo no puede ser mayor que la fecha actual.")]
        public DateOnly? FechaBautizo { get; set; }

        [DisplayName("Dirección")]
        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = null!;

        [DisplayName("Teléfono")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El teléfono debe tener 8 dígitos.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe contener exactamente 8 dígitos sin guiones.")]
        
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

        public virtual ICollection<AsistenciaAlumno> AsistenciaAlumnos { get; set; } = new List<AsistenciaAlumno>();

        public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();

        public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
    }
}
