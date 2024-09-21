using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Proyecto_Facultad.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Facultad.Controllers
{
    [Authorize(Roles = "Coordinador, Admin")]
    public class UsuarioController : Controller
    {
        private readonly BdfflContext _context;

        public UsuarioController(BdfflContext context)
        {
            _context = context;
        }
        // GET: usuario
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
            .Include(u => u.IdRolNavigation) // Incluye la información del rol
            .ToListAsync();

            return View(usuarios);
        }

        // GET: usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            // Cargar la lista de roles para el dropdown
            ViewBag.Roles = new SelectList(_context.Rols, "IdRol", "NombreRol");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreUsuario, ContrasenaUsuario, IdRol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Usar el método SetPassword del modelo Usuario para hashear la contraseña
                usuario.SetPassword(usuario.ContrasenaUsuario);
                usuario.EstadoUsuario = true;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuario creado correctamente";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }
        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Cargar la lista de roles para el dropdown
            ViewBag.Roles = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreUsuario, ContrasenaUsuario, IdRol, EstadoUsuario")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.Usuarios.FindAsync(id);

                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.NombreUsuario = usuario.NombreUsuario;
                    existingUser.IdRol = usuario.IdRol;

                    if (!string.IsNullOrEmpty(usuario.ContrasenaUsuario))
                    {
                        var (hash, salt) = PasswordHasher.HashPassword(usuario.ContrasenaUsuario);
                        existingUser.ContrasenaUsuario = hash;
                        existingUser.ContrasenaUsuario = salt;
                    }

                    _context.Update(existingUser);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["ErrorMessage"] = "Se produjo un error al actualizar los datos.";
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_context.Rols, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Cambiar el estado del usuario
            usuario.EstadoUsuario = !usuario.EstadoUsuario;

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = usuario.EstadoUsuario ? "Usuario activado correctamente" : "Usuario desactivado correctamente";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.IdUsuario))
                {
                    return NotFound();
                }
                else
                {
                    TempData["ErrorMessage"] = "Se produjo un error al cambiar el estado.";
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // Método para verificar si el usuario existe
        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

    }
}
