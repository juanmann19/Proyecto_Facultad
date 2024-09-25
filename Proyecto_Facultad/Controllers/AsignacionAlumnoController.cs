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
    public class AsignacionAlumnoController : Controller
    {
        private readonly BdfflContext _context;

        public AsignacionAlumnoController(BdfflContext context)
        {
            _context = context;
        }

        // GET: AsignacionAlumno
        public async Task<IActionResult> Index()
        {
            var asignacionAlumnos = await _context.AsignacionAlumnos
                 .Include(a => a.IdAlumnoNavigation)
                 .Include(a => a.IdMesaNavigation)
                 .Include(m => m.IdMesaNavigation.NombreSedeNavigation) // Incluir Sede
                 .Include(m => m.IdMesaNavigation.IdJornadaNavigation) // Incluir Jornada
                 .Select(a => new
                 {
                     a.IdAsignacionalumnos,
                     NombreCompletoAlumno = a.IdAlumnoNavigation.PrimerNombreAlumno + " " + a.IdAlumnoNavigation.PrimerApellidoAlumno,
                     Mesa = a.IdMesaNavigation.IdMesa,
                     Sede = a.IdMesaNavigation.NombreSedeNavigation.NombreSede, // Obtener el nombre de la sede
                     Jornada = $"{a.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {a.IdMesaNavigation.IdJornadaNavigation.Horario}" // Obtener jornada
                 })
                .ToListAsync();

            return View(asignacionAlumnos);
        }

        //public async Task<IActionResult> Index()
        //{
        //   var asignacionAlumnos = await _context.AsignacionAlumnos
        //        .Include(a => a.IdAlumnoNavigation)
        //        .Include(a => a.IdMesaNavigation)
        //        .Select(a => new
        //        {
        //           a.IdAsignacionalumnos,
        //           NombreCompletoAlumno = a.IdAlumnoNavigation.PrimerNombreAlumno + " " + a.IdAlumnoNavigation.PrimerApellidoAlumno,
        //           Mesa = a.IdMesaNavigation.IdMesa
        //        })
        //       .ToListAsync();

        //    return View(asignacionAlumnos);
        //}


        // GET: AsignacionAlumno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacionAlumno = await _context.AsignacionAlumnos
                .Include(a => a.IdAlumnoNavigation)
                .Include(a => a.IdMesaNavigation)
                .FirstOrDefaultAsync(m => m.IdAsignacionalumnos == id);
            if (asignacionAlumno == null)
            {
                return NotFound();
            }

            return View(asignacionAlumno);
        }

        // GET: AsignacionAlumno/Create
        public IActionResult Create()
        {
            // Obtener la lista de mesas con información combinada
            var mesas = _context.Mesas
                .Include(m => m.NombreSedeNavigation)
                .Include(m => m.IdJornadaNavigation)
                .Select(m => new
                {
                    m.IdMesa,
                    DisplayText = $"{m.NombreSedeNavigation.NombreSede} - {m.IdJornadaNavigation.DiaSemana} {m.IdJornadaNavigation.Horario}"
                })
                .ToList();

            ViewData["IdMesa"] = new SelectList(mesas, "IdMesa", "DisplayText");

            // Obtener todos los alumnos con su nombre completo
            var alumnos = _context.Alumnos
                .Select(a => new
                {
                    a.IdAlumno,
                    NombreCompleto = $"{a.PrimerNombreAlumno} {a.PrimerApellidoAlumno}"
                })
                .ToList();

            // Verifica si la lista de alumnos no está vacía
            if (alumnos == null || !alumnos.Any())
            {
                // Manejar el caso donde no hay alumnos
                // Puedes retornar una vista con un mensaje o redirigir a otra acción
                ViewBag.IdAlumno = new SelectList(Enumerable.Empty<SelectListItem>(), "IdAlumno", "NombreCompleto");
            }
            else
            {
                ViewBag.IdAlumno = new SelectList(alumnos, "IdAlumno", "NombreCompleto");
            }

            // Obtener la lista de mesas o staff
            ViewBag.IdStaff = new SelectList(_context.Mesas, "IdMesa", "IdMesa");

            return View();
        }


    

        // POST: AsignacionAlumno/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsignacionalumnos,IdAlumno,IdMesa")] AsignacionAlumno asignacionAlumno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asignacionAlumno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", asignacionAlumno.IdAlumno);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asignacionAlumno.IdMesa);
            return View(asignacionAlumno);
        }

        // GET: AsignacionAlumno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacionAlumno = await _context.AsignacionAlumnos
                .Include(a => a.IdAlumnoNavigation)
                .Include(a => a.IdMesaNavigation)
                .FirstOrDefaultAsync(a => a.IdAsignacionalumnos == id);

            if (asignacionAlumno == null)
            {
                return NotFound();
            }

            // Obtener la lista de mesas con información combinada
            var mesas = _context.Mesas
                .Include(m => m.NombreSedeNavigation)
                .Include(m => m.IdJornadaNavigation)
                .Select(m => new
                {
                    m.IdMesa,
                    DisplayText = $"{m.NombreSedeNavigation.NombreSede} - {m.IdJornadaNavigation.DiaSemana} {m.IdJornadaNavigation.Horario}"
                })
                .ToList();

            ViewData["IdMesa"] = new SelectList(mesas, "IdMesa", "DisplayText", asignacionAlumno.IdMesa);

            // Obtener la lista de alumnos
            var alumnos = _context.Alumnos
                .Select(a => new
                {
                    a.IdAlumno,
                    NombreCompleto = $"{a.PrimerNombreAlumno} {a.PrimerApellidoAlumno}"
                })
                .ToList();

            ViewData["IdAlumno"] = new SelectList(alumnos, "IdAlumno", "NombreCompleto", asignacionAlumno.IdAlumno);

            return View(asignacionAlumno);
        }
        //    var asignacionAlumno = await _context.AsignacionAlumnos.FindAsync(id);
        //    if (asignacionAlumno == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", asignacionAlumno.IdAlumno);
        //    ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asignacionAlumno.IdMesa);
        //    return View(asignacionAlumno);
        //}

        // POST: AsignacionAlumno/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsignacionalumnos,IdAlumno,IdMesa")] AsignacionAlumno asignacionAlumno)
        {
            if (id != asignacionAlumno.IdAsignacionalumnos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asignacionAlumno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsignacionAlumnoExists(asignacionAlumno.IdAsignacionalumnos))
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
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", asignacionAlumno.IdAlumno);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asignacionAlumno.IdMesa);
            return View(asignacionAlumno);
        }

        // GET: AsignacionAlumno/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asignacionAlumno = await _context.AsignacionAlumnos
                .Include(a => a.IdAlumnoNavigation)
                .Include(a => a.IdMesaNavigation)
                .FirstOrDefaultAsync(m => m.IdAsignacionalumnos == id);
            if (asignacionAlumno == null)
            {
                return NotFound();
            }

            return View(asignacionAlumno);
        }

        // POST: AsignacionAlumno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asignacionAlumno = await _context.AsignacionAlumnos.FindAsync(id);
            if (asignacionAlumno != null)
            {
                _context.AsignacionAlumnos.Remove(asignacionAlumno);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsignacionAlumnoExists(int id)
        {
            return _context.AsignacionAlumnos.Any(e => e.IdAsignacionalumnos == id);
        }
    }
}
