using System;
using System.Collections.Generic;
namespace Proyecto_Facultad.Models;
using System.ComponentModel;
using Proyecto_Facultad.Helpers;
using System.ComponentModel.DataAnnotations;

public partial class Usuario
{
    public int IdUsuario { get; set; }


    [DisplayName("Nombre Usuario")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Campo puede contener letras y numeros")]
    [Required(ErrorMessage = "El Nombre de usuario es Obligatorio")]

    public string NombreUsuario { get; set; } = null!;

    [DisplayName("Contraseña ")]
    [Required(ErrorMessage = "La contraseña es Obligatoria")]
    public string ContrasenaUsuario { get; set; } = null!;

    [DisplayName("Rol ")]
    [Required(ErrorMessage = "El rol es obligatorio")]
    public int IdRol { get; set; }

    public virtual Rol IdRolNavigation { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
    public void SetPassword(string password)
    {
        var (hash, salt) = PasswordHasher.HashPassword(password);
        ContrasenaUsuario = hash;
        ContrasenaUsuario = salt;
    }

    public bool VerifyPassword(string password)
    {
        return PasswordHasher.VerifyPassword(password, ContrasenaUsuario, ContrasenaUsuario);
    }
}
