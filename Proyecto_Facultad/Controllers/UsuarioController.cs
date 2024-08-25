using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Proyecto_Facultad.Helpers;

namespace Proyecto_Facultad.Controllers
{
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
            return View(await _context.Usuarios.ToListAsync());
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

        // GET: usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreUsuario, ContrasenaUsuario, IdRol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Hashear la contraseña antes de guardar el usuario
                var (hash, salt) = PasswordHasher.HashPassword(usuario.ContrasenaUsuario);
                usuario.ContrasenaUsuario = hash;
                usuario.ContrasenaUsuario = salt;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Datos cargado correctamente";
                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }

        // GET: usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,NombreUsuario, ContrasenaUsuario, IdRol")] Usuario usuario)
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
            return View(usuario);
        }

        // // GET: usuario/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var usuario = await _context.Usuarios
        //         .FirstOrDefaultAsync(m => m.IdUsuario == id);
        //     if (usuario == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(usuario);
        // }

        // // POST: usuario/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var usuario = await _context.Usuarios.FindAsync(id);
        //     if (usuario != null)
        //     {
        //         _context.Usuarios.Remove(usuario);
        //         await _context.SaveChangesAsync();
        //         TempData["SuccessMessage"] = "Usuario eliminado correctamente";
        //     }
        //     else
        //     {
        //         TempData["ErrorMessage"] = "No se pudo encontrar el usuario para eliminar";
        //     }

        //     return RedirectToAction(nameof(Index));
        // }
    }
}
