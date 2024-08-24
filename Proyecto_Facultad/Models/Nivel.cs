using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Nivel
{
    public int IdNivel { get; set; }

    [DisplayName("Nombre del nivel")]
    [Required(ErrorMessage = "El nombre del nivel es obligatorio")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ ]+$", ErrorMessage = "Campo solo puede contener letras")]
    public string NombreNivel { get; set; }

    public virtual ICollection<Mesa> Mesas { get; set; } = new List<Mesa>();
}
