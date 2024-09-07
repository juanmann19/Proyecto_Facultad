using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class AsignacionMaestro
{
    public int IdAsignacionmaestros { get; set; }

    public int IdStaff { get; set; }

    public int IdMesa { get; set; }

    public virtual Mesa IdMesaNavigation { get; set; }

    public virtual Staff IdStaffNavigation { get; set; }

    public string NombreCompletoMaestro
    {
        get
        {
        
            return IdStaffNavigation != null ? $"{IdStaffNavigation.PrimerNombreStaff} {IdStaffNavigation.PrimerApellidoStaff}" : "Desconocido";
        }
    }

    public string Mesa
    {
        get
        {
            if (IdMesaNavigation != null)
            {
                // Asumiendo que IdMesaNavigation tiene una propiedad llamada Jornada
                return $"{IdMesaNavigation.IdMesa} - {IdMesaNavigation.IdJornada}";
            }
            return "Desconocida";
        }
    }
}
