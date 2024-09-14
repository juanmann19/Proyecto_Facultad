using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Proyecto_Facultad.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Coordinador")]
    public class BimestreController : Controller
    {
        private readonly BdfflContext _context;

        public BimestreController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Bimestre
        public async Task<IActionResult> Index()
        {
            return View(await _context.Bimestres.ToListAsync());
        }

        // GET: Bimestre/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bimestre = await _context.Bimestres
               .FirstOrDefaultAsync(m => m.IdBimestre == id);
            if (bimestre == null)
            {
                return NotFound();
            }

            return View(bimestre);
        }

        // GET: Bimestre/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libro/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreBimestre")] Bimestre bimestre)
        {

            try
            {
                _context.Add(bimestre);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
                return View(bimestre);
            }
        }
       // GET: Bimestre/Edit/5
public async Task<IActionResult> Edit(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var bimestre = await _context.Bimestres.FindAsync(id);
    if (bimestre == null)
    {
        return NotFound();
    }
    return View(bimestre);
}

// POST: Bimestre/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, [Bind("IdBimestre,NombreBimestre")] Bimestre bimestre)
{
    if (id != bimestre.IdBimestre)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Update(bimestre);
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
    return View(bimestre);
}

        // GET: Bimestre/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bimestre = await _context.Bimestres
                .FirstOrDefaultAsync(m => m.IdBimestre == id);
            if (bimestre == null)
            {
                return NotFound();
            }

            return View(bimestre);
        }

        // POST: Bimestre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var bimestre = await _context.Bimestres.FindAsync(id);
                if (bimestre != null)
                {
                    _context.Bimestres.Remove(bimestre);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Dato eliminado correctamente";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Se produjo un error al eliminar los datos.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}