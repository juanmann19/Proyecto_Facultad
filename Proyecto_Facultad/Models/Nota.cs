using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Nota
{
    public int IdNota { get; set; }

    [DisplayName("Nombre del alumno")]
     [Required(ErrorMessage = "Todos los campos deben llenarse")]
    public int IdAsignacionalumnos { get; set; }

    [DisplayName("Bimestre")]
    [Required(ErrorMessage = "Todos los campos deben llenarse")]
    public int IdBimestre { get; set; }

    [DisplayName("Nota Obtenida en el examen")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Campo solo puede contener numeros")]
    [Required(ErrorMessage = "Todos los campos deben llenarse")]
    [Range(0, 100, ErrorMessage = "La nota debe estar entre 0 y 100.")]
    public int NotaObtenida { get; set; }

    public virtual AsignacionAlumno IdAsignacionalumnosNavigation { get; set; }

    public virtual Bimestre IdBimestreNavigation { get; set; }
}
