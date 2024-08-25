using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Bimestre
{
    public int IdBimestre { get; set; }


    [DisplayName("Nombre Bimestre")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚÜüñÑ0-9 ]+$", ErrorMessage = "Campo puede contener letras y numeros")]
    [Required(ErrorMessage = "El Nombre de Lección es Obligatorio")]
    public string NombreBimestre { get; set; } = null!;

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<Notum> Nota { get; set; } = new List<Notum>();
}
