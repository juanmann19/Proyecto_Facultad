using System;
using System.Collections.Generic;

namespace Proyecto_Facultad.Models;

public partial class Modulo
{
    public int IdModulo { get; set; }

    public string NombreModulo { get; set; }

    public virtual ICollection<ModulosRole> ModulosRoles { get; set; } = new List<ModulosRole>();
}
