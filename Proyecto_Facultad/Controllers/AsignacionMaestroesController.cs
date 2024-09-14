using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Facultad.ViewModels;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Coordinador")]
    public class AsignacionMaestroesController : Controller
    {
        private readonly BdfflContext _context;

        public AsignacionMaestroesController(BdfflContext context)
        {
            _context = context;
        }

        // Acción para mostrar el reporte
        public async Task<IActionResult> Reporte(string searchTerm)
        {
            var maestros = await _context.Staff
                .Include(s => s.AsignacionMaestros)
                .Select(s => new MaestroInfo
                {
                    IdStaff = s.IdStaff,
                    NombreCompleto = $"{s.PrimerNombreStaff} {s.PrimerApellidoStaff}",
                    EstaAsignado = s.AsignacionMaestros.Any()
                })
                .ToListAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                maestros = maestros
                    .Where(m => m.NombreCompleto.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            // Para el estado "Asignado" o "No asignado"
            foreach (var maestro in maestros)
            {
                maestro.EstaAsignado = _context.AsignacionMaestros.Any(a => a.IdStaff == maestro.IdStaff);
            }

            var modelo = new ReporteMaestrosViewModel
            {
                MaestrosAsignados = maestros.Count(m => m.EstaAsignado),
                MaestrosDisponibles = maestros.Count(m => !m.EstaAsignado),
                MaestrosNoAsignados = maestros.Where(m => !m.EstaAsignado).ToList(),
                SearchTerm = searchTerm
            };

            return View(modelo);
        }
        // GET: AsignacionMaestroes
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.AsignacionMaestros.Include(a => a.IdMesaNavigation).Include(a => a.IdStaffNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: AsignacionMaestroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacionMaestro = await _context.AsignacionMaestros
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdAsignacionmaestros == id);
            if (asignacionMaestro == null)
            {
                return NotFound();
            }

            return View(asignacionMaestro);
        }

        // GET: AsignacionMaestroes/Create
        public IActionResult Create()
        {
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa");
            ViewData["IdStaff"] = new SelectList(_context.Staff.Select(s => new
            {
                s.IdStaff,
                NombreCompleto = $"{s.PrimerNombreStaff} {s.PrimerApellidoStaff}"
            }), "IdStaff", "NombreCompleto");

            return View();
        }

        // POST: AsignacionMaestroes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsignacionmaestros,IdStaff,IdMesa")] AsignacionMaestro asignacionMaestro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asignacionMaestro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asignacionMaestro.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", asignacionMaestro.IdStaff);
            return View(asignacionMaestro);
        }

        // GET: AsignacionMaestroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacionMaestro = await _context.AsignacionMaestros.FindAsync(id);
            if (asignacionMaestro == null)
            {
                return NotFound();
            }
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asignacionMaestro.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", asignacionMaestro.IdStaff);
            return View(asignacionMaestro);
        }

        // POST: AsignacionMaestroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsignacionmaestros,IdStaff,IdMesa")] AsignacionMaestro asignacionMaestro)
        {
            if (id != asignacionMaestro.IdAsignacionmaestros)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asignacionMaestro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsignacionMaestroExists(asignacionMaestro.IdAsignacionmaestros))
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
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asignacionMaestro.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", asignacionMaestro.IdStaff);
            return View(asignacionMaestro);
        }

        // GET: AsignacionMaestroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacionMaestro = await _context.AsignacionMaestros
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdAsignacionmaestros == id);
            if (asignacionMaestro == null)
            {
                return NotFound();
            }

            return View(asignacionMaestro);
        }

        // POST: AsignacionMaestroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asignacionMaestro = await _context.AsignacionMaestros.FindAsync(id);
            if (asignacionMaestro != null)
            {
                _context.AsignacionMaestros.Remove(asignacionMaestro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsignacionMaestroExists(int id)
        {
            return _context.AsignacionMaestros.Any(e => e.IdAsignacionmaestros == id);
        }
    }
}
