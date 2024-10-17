using System;
using System.Collections.Generic;
using System.ComponentModel;
using Proyecto_Facultad.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Facultad.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

<<<<<<< HEAD
    [DisplayName("Nombre de usuario")]
=======
    [DisplayName("Nombre usuario")]
>>>>>>> df9eb81c9d1907581ce522d4cd1b9dbea105eec1
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Campo puede contener letras y numeros")]
    [Required(ErrorMessage = "El Nombre de usuario es Obligatorio")]
    public string NombreUsuario { get; set; }
    
    [DisplayName("Contraseña ")]
    [Required(ErrorMessage = "La contraseña es Obligatoria")]
    public string ContrasenaUsuario { get; set; }

    [DisplayName("Rol ")]
    [Required(ErrorMessage = "El rol es obligatorio")]
    public int IdRol { get; set; }

    [DisplayName("Estado usuario")]
    public bool EstadoUsuario { get; set; }
   
    public virtual Rol IdRolNavigation { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
    
    public void SetPassword(string password)
    {
        var (hash, salt) = PasswordHasher.HashPassword(password);
        ContrasenaUsuario = $"{hash}:{salt}";
    }

    public bool VerifyPassword(string password)
    {
        var parts = ContrasenaUsuario.Split(':');
        if (parts.Length != 2) return false;

        var storedHash = parts[0];
        var storedSalt = parts[1];

        return PasswordHasher.VerifyPassword(password, storedHash, storedSalt);
    }

}
