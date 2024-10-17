﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Libro
{
    public int IdLibro { get; set; }

    [DisplayName("Nombre del libro")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ0-9 ]+$", ErrorMessage = "Campo puede contener letras y numeros")]
    [Required(ErrorMessage = "El Nombre de Libro es Obligatorio")]
    public string NombreLibro { get; set; }

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();

    public virtual ICollection<Leccion> Leccions { get; set; } = new List<Leccion>();
}
