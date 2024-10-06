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
    
    [Authorize (Roles = "Maestro, Admin")]
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
            var bdfflContext = _context.Notas.Include(n => n.IdAsignacionalumnosNavigation).ThenInclude(aa => aa.IdAlumnoNavigation).Include(n => n.IdBimestreNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: Nota/Details/5
        // public async Task<IActionResult> Details(int? id)
        //{
        //   if (id == null)
        //     {
        //         return NotFound();
        //     }

        //         var nota = await _context.Notas
        //         .Include(n => n.IdAsignacionalumnosNavigation)
        //         .Include(n => n.IdBimestreNavigation)
        //       .FirstOrDefaultAsync(m => m.IdNota == id);
        //     if (nota == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(nota);
        // }

        // GET: Nota/Create
        public IActionResult Create()
        {
            // Obtener las asignaciones de alumnos con su nombre completo
            var asignaciones = _context.AsignacionAlumnos
                .Include(a => a.IdAlumnoNavigation)  // Incluir la navegación hacia el alumno
                .Select(a => new
                {
                    a.IdAsignacionalumnos,  // Utilizar IdAsignacionalumnos como valor
                    NombreCompleto = $"{a.IdAlumnoNavigation.PrimerNombreAlumno} {a.IdAlumnoNavigation.PrimerApellidoAlumno}"
                })
                .ToList();

            // Pasar la lista de asignaciones de alumnos a la vista
            ViewBag.IdAlumno = new SelectList(asignaciones, "IdAsignacionalumnos", "NombreCompleto");

            // Cargar bimestres en el dropdown
            ViewBag.IdBimestre = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");

            return View();
        }
        //public IActionResult Create()
        //{
        //    // Obtener todas las mesas
        //    var mesas = _context.Mesas
        //        .Select(m => new
        //        {
        //            m.IdMesa,
        //            Descripcion = m.IdMesa.ToString() // Ajusta esto según tu modelo de mesa
        //        })
        //        .ToList();

        //    // Obtener todos los alumnos
        //    var alumnos = _context.Alumnos
        //        .Select(a => new
        //        {
        //            a.IdAlumno,
        //            NombreCompleto = $"{a.PrimerNombreAlumno} {a.PrimerApellidoAlumno}"
        //        })
        //        .ToList();

        //    // Pasar las mesas y alumnos a la vista
        //    ViewBag.IdMesa = new SelectList(mesas, "IdMesa", "Descripcion");
        //    ViewBag.IdAlumno = new SelectList(alumnos, "IdAlumno", "NombreCompleto");

        //    // Obtener bimestres
        //    ViewBag.IdBimestre = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");

        //    return View();
        //}


        // POST: Nota/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create([Bind("IdNota,IdAsignacionalumnos,IdBimestre,NotaObtenida")] Nota nota)
{
    // Validar manualmente el rango de la nota
    if (nota.NotaObtenida < 0)
    {
        ModelState.AddModelError("NotaObtenida", "La nota debe ser mayor a 0.");
    }
    else if (nota.NotaObtenida > 100)
    {
        ModelState.AddModelError("NotaObtenida", "La nota debe ser menor o igual a 100.");
    }

    if (ModelState.IsValid)
    {
        try
        {
            _context.Add(nota);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Nota asignada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Ocurrió un error al guardar los datos: " + ex.Message;
        }
    }

    // Si la validación falla, recargar los dropdowns
    var asignaciones = _context.AsignacionAlumnos
        .Include(a => a.IdAlumnoNavigation)
        .Select(a => new
        {
            a.IdAsignacionalumnos,
            NombreCompleto = $"{a.IdAlumnoNavigation.PrimerNombreAlumno} {a.IdAlumnoNavigation.PrimerApellidoAlumno}"
        })
        .ToList();

    ViewBag.IdAlumno = new SelectList(asignaciones, "IdAsignacionalumnos", "NombreCompleto", nota.IdAsignacionalumnos);
    ViewBag.IdBimestre = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", nota.IdBimestre);

    return View(nota);
}

        //public async Task<IActionResult> Create([Bind("IdNota,IdAsignacionalumnos,IdBimestre,NotaObtenida")] Nota nota)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(nota);
        //        await _context.SaveChangesAsync();
        //        TempData["SuccessMessage"] = "Dato cargado correctamente";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdAsignacionalumnos"] = new SelectList(_context.AsignacionAlumnos, "IdAlumno", "PrimerNombreAlumno", nota.IdAsignacionalumnos);
        //    ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", nota.IdBimestre);
        //    TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
        //    return View(nota);
        //}

        // GET: Nota/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nota = await _context.Notas
                .Include(n => n.IdAsignacionalumnosNavigation)  // Asegúrate de incluir la navegación
                .ThenInclude(a => a.IdAlumnoNavigation)         // Incluir la navegación hacia Alumno
                .FirstOrDefaultAsync(n => n.IdNota == id);

            if (nota == null)
            {
                return NotFound();
            }

            // Obtener la lista de alumnos con su nombre completo
            var alumnos = await _context.AsignacionAlumnos
                .Include(a => a.IdAlumnoNavigation) // Asegúrate de incluir la navegación hacia Alumno
                .Select(a => new
                {
                    a.IdAsignacionalumnos,
                    NombreCompleto = $"{a.IdAlumnoNavigation.PrimerNombreAlumno} {a.IdAlumnoNavigation.PrimerApellidoAlumno}"
                })
                .ToListAsync();

            // Pasar la lista de alumnos a la vista
            ViewData["IdAsignacionalumnos"] = new SelectList(alumnos, "IdAsignacionalumnos", "NombreCompleto", nota.IdAsignacionalumnos);

            // Obtener los bimestres
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", nota.IdBimestre);

            return View(nota);
        }

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var nota = await _context.Notas.FindAsync(id);
        //    if (nota == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdAsignacionalumnos"] = new SelectList(_context.AsignacionAlumnos, "IdAsignacionalumnos", "IdAsignacionalumnos", nota.IdAsignacionalumnos);
        //    ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", nota.IdBimestre);
        //    return View(nota);
        //}

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
