using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Nota
{
    public int IdNota { get; set; }

    [DisplayName("Nombre del alumno")]
    public int IdAsignacionalumnos { get; set; }

    [DisplayName("Bimestre")]
    public int IdBimestre { get; set; }

    [DisplayName("Nota Obtenida en el examen")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Campo solo puede contener numeros")]
    public int NotaObtenida { get; set; }

    public virtual AsignacionAlumno IdAsignacionalumnosNavigation { get; set; }

    public virtual Bimestre IdBimestreNavigation { get; set; }
}
