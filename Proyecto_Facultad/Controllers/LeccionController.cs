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
    public class LeccionController : Controller
    {
        private readonly BdfflContext _context;

        public LeccionController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Leccion
        public async Task<IActionResult> Index()
        {
            return View(await _context.Leccions.ToListAsync());
        }

        // GET: Leccion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Leccions
               .FirstOrDefaultAsync(m => m.IdLeccion == id);
            if (leccion == null)
            {
                return NotFound();
            }

            return View(leccion);
        }

        // GET: Leccion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leccion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion,IdLibro")] Leccion leccion)
        {

            try
            {
                _context.Add(leccion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
                return View(leccion);
            }
        }
       // GET: Leccion/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var leccion = await _context.Leccions.FindAsync(id);
    if (leccion == null)
    {
        return NotFound();
    }
    return View(leccion);
}

// POST: Leccion/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("IdLeccion,Descripcion,IdLibro")] Leccion leccion)
{
    if (id != leccion.IdLeccion)
    {
        return RedirectToAction(nameof(Index));
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(leccion);
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
    return View(leccion);
}

        // GET: Leccion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Leccions
                .FirstOrDefaultAsync(m => m.IdLeccion == id);
            if (leccion == null)
            {
                return NotFound();
            }

            return View(leccion);
        }

        // POST: Leccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var leccion = await _context.Leccions.FindAsync(id);
                if (leccion != null)
                {
                    _context.Leccions.Remove(leccion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Lección eliminada correctamente";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Se produjo un error al eliminar la lección.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
