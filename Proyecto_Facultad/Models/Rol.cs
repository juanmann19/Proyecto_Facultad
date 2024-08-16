using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Rol
{
    public int IdRol { get; set; }

    public string NombreRol { get; set; } = null!;

    public virtual ICollection<ModulosRole> ModulosRoles { get; set; } = new List<ModulosRole>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
