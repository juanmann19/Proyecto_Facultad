using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Staff
{
    public int IdStaff { get; set; }

    public string PrimerNombreStaff { get; set; }

    public string SegundoNombreStaff { get; set; }

    public string OtrosNombresStaff { get; set; }

    public string PrimerApellidoStaff { get; set; }

    public string SegundoApellidoStaff { get; set; }

    public string ApellidoCasado { get; set; }

    public DateOnly FechaNacimientoStaff { get; set; }

    public DateOnly? FechaBautizoStaff { get; set; }

    public string Direccion { get; set; }

    public string Telefono { get; set; }

    public int? Dpi { get; set; }

    public string Nit { get; set; }

    public string EstadoCivil { get; set; }

    public string Genero { get; set; }

    public string NumeroCelula { get; set; }

    public string NivelAprobado { get; set; }

    public string CodigoFrater { get; set; }

    public bool EstatusStaff { get; set; }

    public int IdUsuario { get; set; }

    public virtual ICollection<AsignacionMaestro> AsignacionMaestros { get; set; } = new List<AsignacionMaestro>();

    public virtual ICollection<AsistenciaStaff> AsistenciaStaffs { get; set; } = new List<AsistenciaStaff>();

    public virtual ICollection<Capacitacion> Capacitacions { get; set; } = new List<Capacitacion>();

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
