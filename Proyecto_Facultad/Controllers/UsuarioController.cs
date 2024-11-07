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
        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            // Obtiene la lista de usuarios y la ordena por NombreUsuario
            var usuarios = await _context.Usuarios
                .Include(u => u.IdRolNavigation) // Incluye la información del rol
                .OrderBy(u => u.NombreUsuario)   // Ordena por NombreUsuario
                .ToListAsync(); // Convierte la consulta a una lista

            return View(usuarios); // Envía la lista ordenada a la vista
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

                    if (ModelState.IsValid)
                    {
                        // Usar el método SetPassword del modelo Usuario para hashear la contraseña
                        existingUser.SetPassword(usuario.ContrasenaUsuario);
                        existingUser.EstadoUsuario = true;

                        _context.Update(existingUser);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Usuario actualizado correctamente";
                        //return RedirectToAction(nameof(Index));
                    }
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "El ID del usuario no es válido.";
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                TempData["ErrorMessage"] = "El usuario no se encontró.";
                return NotFound();
            }

            // Cambiar el estatus a desactivado
            usuario.EstadoUsuario = false;

            try
            {
                _context.Update(usuario); // Asegúrate de actualizar el estado en el contexto
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuario inactivado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.IdUsuario))
                {
                    TempData["ErrorMessage"] = "Error al inactivar el usuario.";
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "El ID del usuario no es válido.";
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                TempData["ErrorMessage"] = "El usuario no se encontró.";
                return NotFound();
            }

            // Cambiar el estatus a activado
            usuario.EstadoUsuario = true;

            try
            {
                _context.Update(usuario); // Asegúrate de actualizar el estado en el contexto
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Usuario activado correctamente.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.IdUsuario))
                {
                    TempData["ErrorMessage"] = "Error al activar el usuario.";
                    return NotFound();
                }
                else
                {
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
