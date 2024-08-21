using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Libro
{
    public int IdLibro { get; set; }

    
    [DisplayName("Nombre Libro")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ0-9 ]+$", ErrorMessage = "Campo puede contener letras y numeros")]
    [Required(ErrorMessage = "El Nombre de Libro es Obligatorio")]
    public string NombreLibro { get; set; } = null!;

    public virtual ICollection<Leccion> Leccions { get; set; } = new List<Leccion>();
}
