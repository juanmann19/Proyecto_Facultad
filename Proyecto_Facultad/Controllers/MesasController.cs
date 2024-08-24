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
    public class MesasController : Controller
    {
        private readonly BdfflContext _context;

        public MesasController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Mesas
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.Mesas
                .Include(m => m.IdAlumnoNavigation)
                .Include(m => m.IdJornadaNavigation)
                .Include(m => m.IdNivelNavigation)
                .Include(m => m.IdStaffNavigation);
            return View(await bdfflContext.ToListAsync());
        }



        // GET: Mesas/Create
        public IActionResult Create()
        {
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno");
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "IdJornada", "DiaJornada");
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "NombreNivel");
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff");
            return View();
        }

        // POST: Mesas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMesa,FechaInicio,IdStaff,IdAlumno,IdJornada,IdNivel,AnioAsignacion,EstadoMesa")] Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", mesa.IdAlumno);
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "IdJornada", "DiaJornada", mesa.IdJornada);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "NombreNivel", mesa.IdNivel);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff", mesa.IdStaff);
            return View(mesa);
        }

        // GET: Mesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", mesa.IdAlumno);
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "IdJornada", "DiaJornada", mesa.IdJornada);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "NombreNivel", mesa.IdNivel);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff", mesa.IdStaff);
            return View(mesa);
        }
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMesa,FechaInicio,IdStaff,IdAlumno,IdJornada,IdNivel,AnioAsignacion,EstadoMesa")] Mesa mesa)
        {
            if (id != mesa.IdMesa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesaExists(mesa.IdMesa))
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
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", mesa.IdAlumno);
            ViewData["IdJornada"] = new SelectList(_context.Jornada, "IdJornada", "DiaJornada", mesa.IdJornada);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "NombreNivel", mesa.IdNivel);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff", mesa.IdStaff);
            return View(mesa);
        }

       
        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.IdMesa == id);
        }
    }
}
