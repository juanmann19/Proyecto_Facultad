using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? NombreUsuario { get; set; }

    public string? ContrasenaUsuario { get; set; }

    public int? IdRol { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
