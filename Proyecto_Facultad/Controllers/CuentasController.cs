using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Facultad.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

public class CuentasController : Controller
{
    private readonly BdfflContext _context;

    public CuentasController(BdfflContext context)
    {
        _context = context;
    }

    // GET: Cuentas/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST: Cuentas/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string nombreUsuario, string contrasenaUsuario)
    {
        if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contrasenaUsuario))
        {
            ModelState.AddModelError(string.Empty, "Los campos son obligatorios.");
            return View();
        }

        var usuario = await _context.Usuarios
            .Include(u => u.IdRolNavigation)
            .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);

        if (usuario == null || !usuario.VerifyPassword(contrasenaUsuario))
        {
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.NombreUsuario),
            new Claim(ClaimTypes.Role, usuario.IdRolNavigation.NombreRol)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        return RedirectToAction("Index", "Home");
    }

    // GET: Cuentas/Logout
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Cuentas");
    }
}
