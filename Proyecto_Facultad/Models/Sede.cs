using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Sede
{
    public int IdSede { get; set; }
    [Required]
    [Display(Name = "Nombre de la Sede")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ\s]+$", ErrorMessage = "Campo solo puede contener letras.")]
    public string NombreSede { get; set; }

    public virtual ICollection<Jornadum> Jornada { get; set; } = new List<Jornadum>();

}
