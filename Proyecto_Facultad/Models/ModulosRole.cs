using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class ModulosRole
{
    public int IdModulosRoles { get; set; }

    public int? IdRol { get; set; }

    public int? IdModulo { get; set; }

    public virtual Modulo? IdModuloNavigation { get; set; }

    public virtual Rol? IdRolNavigation { get; set; }
}
