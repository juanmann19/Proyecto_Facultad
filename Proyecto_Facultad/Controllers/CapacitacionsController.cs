using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Facultad.Controllers
{
    
    public class CapacitacionsController : Controller
    {
        private readonly BdfflContext _context;

        public CapacitacionsController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Capacitacions
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.Capacitacions.Include(c => c.IdStaffNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: Capacitacions/Details/5
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacion = await _context.Capacitacions
                .Include(c => c.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdCapacitacion == id);
            if (capacitacion == null)
            {
                return NotFound();
            }

            return View(capacitacion);
        }

        // GET: Capacitacions/Create
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public IActionResult Create()
        {
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado");
            return View();
        }

        // POST: Capacitacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public async Task<IActionResult> Create([Bind("IdCapacitacion,FechaCapacitacion,IdStaff")] Capacitacion capacitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(capacitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", capacitacion.IdStaff);
            return View(capacitacion);
        }

        // GET: Capacitacions/Edit/5
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacion = await _context.Capacitacions.FindAsync(id);
            if (capacitacion == null)
            {
                return NotFound();
            }
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", capacitacion.IdStaff);
            return View(capacitacion);
        }

        // POST: Capacitacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public async Task<IActionResult> Edit(int id, [Bind("IdCapacitacion,FechaCapacitacion,IdStaff")] Capacitacion capacitacion)
        {
            if (id != capacitacion.IdCapacitacion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(capacitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapacitacionExists(capacitacion.IdCapacitacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", capacitacion.IdStaff);
            return View(capacitacion);
        }

        // GET: Capacitacions/Delete/5
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacion = await _context.Capacitacions
                .Include(c => c.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdCapacitacion == id);
            if (capacitacion == null)
            {
                return NotFound();
            }

            return View(capacitacion);
        }

        // POST: Capacitacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Coordinador, Auxiliar")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var capacitacion = await _context.Capacitacions.FindAsync(id);
            if (capacitacion != null)
            {
                _context.Capacitacions.Remove(capacitacion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapacitacionExists(int id)
        {
            return _context.Capacitacions.Any(e => e.IdCapacitacion == id);
        }
    }
}
