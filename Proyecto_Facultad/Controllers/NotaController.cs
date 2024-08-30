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
    public class NotaController : Controller
    {
        private readonly BdfflContext _context;

        public NotaController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Nota
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.Notas.Include(n => n.IdAsignacionalumnosNavigation).Include(n => n.IdBimestreNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: Nota/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nota = await _context.Notas
                .Include(n => n.IdAsignacionalumnosNavigation)
                .Include(n => n.IdBimestreNavigation)
                .FirstOrDefaultAsync(m => m.IdNota == id);
            if (nota == null)
            {
                return NotFound();
            }

            return View(nota);
        }

        // GET: Nota/Create
        public IActionResult Create()
        {
            ViewData["IdAsignacionalumnos"] = new SelectList(_context.AsignacionAlumnos, "IdAsignacionalumnos", "IdAsignacionalumnos");
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");
            return View();
        }

        // POST: Nota/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNota,IdAsignacionalumnos,IdBimestre,NotaObtenida")] Nota nota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nota);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAsignacionalumnos"] = new SelectList(_context.AsignacionAlumnos, "IdAsignacionalumnos", "IdAsignacionalumnos", nota.IdAsignacionalumnos);
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", nota.IdBimestre);
            TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
            return View(nota);
        }

        // GET: Nota/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nota = await _context.Notas.FindAsync(id);
            if (nota == null)
            {
                return NotFound();
            }
            ViewData["IdAsignacionalumnos"] = new SelectList(_context.AsignacionAlumnos, "IdAsignacionalumnos", "IdAsignacionalumnos", nota.IdAsignacionalumnos);
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", nota.IdBimestre);
            return View(nota);
        }

        // POST: Nota/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNota,IdAsignacionalumnos,IdBimestre,NotaObtenida")] Nota nota)
        {
            if (id != nota.IdNota)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nota);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NotaExists(nota.IdNota))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Se produjo un error al actualizar los datos.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAsignacionalumnos"] = new SelectList(_context.AsignacionAlumnos, "IdAsignacionalumnos", "IdAsignacionalumnos", nota.IdAsignacionalumnos);
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", nota.IdBimestre);
            return View(nota);
        }

        // GET: Nota/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nota = await _context.Notas
                .Include(n => n.IdAsignacionalumnosNavigation)
                .Include(n => n.IdBimestreNavigation)
                .FirstOrDefaultAsync(m => m.IdNota == id);
            if (nota == null)
            {
                return NotFound();
            }

            return View(nota);
        }

        // POST: Nota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nota = await _context.Notas.FindAsync(id);
            if (nota != null)
            {
                _context.Notas.Remove(nota);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NotaExists(int id)
        {
            return _context.Notas.Any(e => e.IdNota == id);
        }
    }
}
