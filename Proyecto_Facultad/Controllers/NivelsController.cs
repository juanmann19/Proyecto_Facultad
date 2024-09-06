using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;

namespace Proyecto_Facultad.Controllers
{
    public class NivelsController : Controller
    {
        private readonly BdfflContext _context;

        public NivelsController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Nivels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nivels.ToListAsync());
        }

        // GET: Nivels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nivels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNivel,NombreNivel")] Nivel nivel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nivel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Nivel creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error al crear el nivel.";
            return View(nivel);
        }

        // GET: Nivels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID no válido.";
                return NotFound();
            }

            var nivel = await _context.Nivels.FindAsync(id);
            if (nivel == null)
            {
                TempData["ErrorMessage"] = "Nivel no encontrado.";
                return NotFound();
            }
            return View(nivel);
        }

        // POST: Nivels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNivel,NombreNivel")] Nivel nivel)
        {
            if (id != nivel.IdNivel)
            {
                TempData["ErrorMessage"] = "ID no coincide.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nivel);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NivelExists(nivel.IdNivel))
                    {
                        TempData["ErrorMessage"] = "El nivel no existe.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Se produjo un error al actualizar.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error al actualizar el nivel.";
            return View(nivel);
        }
        // GET: Nivels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "ID no válido.";
                return NotFound();
            }

            var nivel = await _context.Nivels
                .FirstOrDefaultAsync(m => m.IdNivel == id);
            if (nivel == null)
            {
                TempData["ErrorMessage"] = "Nivel no encontrado.";
                return NotFound();
            }

            return View(nivel);
        }

        // POST: Nivels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nivel = await _context.Nivels.FindAsync(id);
            if (nivel == null)
            {
                TempData["ErrorMessage"] = "Nivel no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Nivels.Remove(nivel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Nivel eliminado exitosamente.";
            }
            catch (DbUpdateException ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el nivel: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NivelExists(int id)
        {
            return _context.Nivels.Any(e => e.IdNivel == id);
        }
    }
}