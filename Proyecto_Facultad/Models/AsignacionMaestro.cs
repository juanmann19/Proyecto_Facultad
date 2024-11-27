using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Proyecto_Facultad.Models;
public partial class AsignacionMaestro
{
    public int IdAsignacionmaestros { get; set; }

    [DisplayName("Nombre del personal")]
    public int IdStaff { get; set; }

    [DisplayName("Número de mesa")]
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
               
                return $"{IdMesaNavigation.IdMesa} - {IdMesaNavigation.IdJornada}";
            }
            return "Desconocida";
        }
    }
  
}
