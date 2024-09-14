using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using System.Security.Claims;

namespace Proyecto_Facultad.Controllers
{
    public class CuentasController : Controller
    {
        private readonly BdfflContext _context;

        public CuentasController(BdfflContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string nombreUsuario, string contrasenaUsuario)
        {
            // Verificar si los campos están vacíos
            if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasenaUsuario))
            {
                ModelState.AddModelError(string.Empty, "Los campos son obligatorios.");
                return View();
            }

            // Buscar usuario en la base de datos
            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation) // Asegurarse de incluir la navegación de roles
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.EstadoUsuario);

            // Verificar si el usuario es nulo
            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario no encontrado o inactivo.");
                return View();
            }

            // Verificar contraseña
            if (!usuario.VerifyPassword(contrasenaUsuario))
            {
                ModelState.AddModelError(string.Empty, "Contraseña incorrecta.");
                return View();
            }

            // Crear las claims para el usuario autenticado
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
        new Claim(ClaimTypes.Role, usuario.IdRolNavigation?.NombreRol ?? "Usuario") // Manejar caso donde IdRolNavigation es null
    };

            // Crear la identidad y el principal
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            // Autenticar al usuario
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Redirigir al usuario a la página de inicio
            return RedirectToAction("Index", "Home");
        }


        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuentas");
        }
    }
}
