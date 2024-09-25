using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;

public partial class Staff
{
    public int IdStaff { get; set; }

    [DisplayName("Primer Nombre")]
    public string PrimerNombreStaff { get; set; }

    [DisplayName("Segundo Nombre")]
    public string SegundoNombreStaff { get; set; }

    [DisplayName("Otro Nombre")]
    public string OtrosNombresStaff { get; set; }

    [DisplayName("Primer Apellido")]
    public string PrimerApellidoStaff { get; set; }

    [DisplayName("Segundo Apellido")]
    public string SegundoApellidoStaff { get; set; }

    [DisplayName("Apellido de Cadado")]
    public string ApellidoCasado { get; set; }

    [DisplayName("Fecha de Nacimiento")]
    public DateOnly FechaNacimientoStaff { get; set; }

    [DisplayName("Fecha de Bautizo")]
    public DateOnly? FechaBautizoStaff { get; set; }

    public string Direccion { get; set; }

    public string Telefono { get; set; }

    [DisplayName("DPI")]
    public string Dpi { get; set; }

    [DisplayName("NIT")]
    public string Nit { get; set; }

    [DisplayName("Estado Civil")]
    public string EstadoCivil { get; set; }

    public string Genero { get; set; }

    [DisplayName("Pertenece a la Red")]
    public string NumeroCelula { get; set; }

    [DisplayName("Nivel Aprobado")]
    public string NivelAprobado { get; set; }

    [DisplayName("Codigo Frater")]
    public string CodigoFrater { get; set; }

    [DisplayName("Estatus")]
    public bool EstatusStaff { get; set; }

    [DisplayName("Usuario")]
    public int IdUsuario { get; set; }

   
    public virtual ICollection<AsignacionMaestro> AsignacionMaestros { get; set; } = new List<AsignacionMaestro>();

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<Capacitacion> Capacitacions { get; set; } = new List<Capacitacion>();

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
