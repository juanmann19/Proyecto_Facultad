using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Proyecto_Facultad.Models;

public partial class AvanceMesa
{
    public int IdAvancemesa { get; set; }

    [DisplayName("Nivel")]
    [Required(ErrorMessage = "El Nivel es Obligatorio")]
    public int IdNivel { get; set; }
    
    [DisplayName("Bimestre")]
    [Required(ErrorMessage = "El Bimestre es Obligatorio")]
    public int IdBimestre { get; set; }
    
    [DisplayName("Libro")]
    [Required(ErrorMessage = "El Libro es Obligatorio")]
    public int IdLibro { get; set; }
    
    [DisplayName("Leccion")]
    [Required(ErrorMessage = "La lección es Obligatoria")]
    public int IdLeccion { get; set; }

    
    [DisplayName("Mesa")]
    [Required(ErrorMessage = "La Mesa es Obligatoria")]
    public int IdMesa { get; set; }

    public virtual Bimestre IdBimestreNavigation { get; set; }

    public virtual Leccion IdLeccionNavigation { get; set; }

    public virtual Libro IdLibroNavigation { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; }

    public virtual Nivel IdNivelNavigation { get; set; }
}
