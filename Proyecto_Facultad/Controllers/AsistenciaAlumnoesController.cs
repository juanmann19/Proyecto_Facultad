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
    public class AsistenciaAlumnoesController : Controller
    {
        private readonly BdfflContext _context;

        public AsistenciaAlumnoesController(BdfflContext context)
        {
            _context = context;
        }

        // GET: AsistenciaAlumnoes
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.AsistenciaAlumnos.Include(a => a.IdAlumnoNavigation).Include(a => a.IdAsistenciaStaffNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: AsistenciaAlumnoes/Create
        public IActionResult Create()
        {
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "Direccion");
            ViewData["IdAsistenciaStaff"] = new SelectList(_context.AsistenciaStaffs, "IdAsistenciaStaff", "IdAsistenciaStaff");
            return View();
        }

        // POST: AsistenciaAlumnoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsistenciaAlumno,IdAsistenciaStaff,IdAlumno,Ausencia")] AsistenciaAlumno asistenciaAlumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asistenciaAlumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "Direccion", asistenciaAlumno.IdAlumno);
            ViewData["IdAsistenciaStaff"] = new SelectList(_context.AsistenciaStaffs, "IdAsistenciaStaff", "IdAsistenciaStaff", asistenciaAlumno.IdAsistenciaStaff);
            return View(asistenciaAlumno);
        }

        // GET: AsistenciaAlumnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaAlumno = await _context.AsistenciaAlumnos.FindAsync(id);
            if (asistenciaAlumno == null)
            {
                return NotFound();
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "Direccion", asistenciaAlumno.IdAlumno);
            ViewData["IdAsistenciaStaff"] = new SelectList(_context.AsistenciaStaffs, "IdAsistenciaStaff", "IdAsistenciaStaff", asistenciaAlumno.IdAsistenciaStaff);
            return View(asistenciaAlumno);
        }

        // POST: AsistenciaAlumnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsistenciaAlumno,IdAsistenciaStaff,IdAlumno,Ausencia")] AsistenciaAlumno asistenciaAlumno)
        {
            if (id != asistenciaAlumno.IdAsistenciaAlumno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistenciaAlumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaAlumnoExists(asistenciaAlumno.IdAsistenciaAlumno))
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
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "Direccion", asistenciaAlumno.IdAlumno);
            ViewData["IdAsistenciaStaff"] = new SelectList(_context.AsistenciaStaffs, "IdAsistenciaStaff", "IdAsistenciaStaff", asistenciaAlumno.IdAsistenciaStaff);
            return View(asistenciaAlumno);
        }
        private bool AsistenciaAlumnoExists(int id)
        {
            return _context.AsistenciaAlumnos.Any(e => e.IdAsistenciaAlumno == id);
        }
    }
}
