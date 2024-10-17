using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Leccion
{
    public int IdLeccion { get; set; }

    [DisplayName("Nombre de leccion")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ0-9 ]+$", ErrorMessage = "Campo puede contener letras y numeros")]
    [Required(ErrorMessage = "El Nombre de lección es obligatorio")]
    public string Descripcion { get; set; }

    [DisplayName("Codigo del libro")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "Campo puede contener numeros")]
    [Required(ErrorMessage = "El Libro es obligatorio")]
    public int IdLibro { get; set; }

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<AvanceMesa> AvanceMesas { get; set; } = new List<AvanceMesa>();

    public virtual Libro IdLibroNavigation { get; set; }
}
