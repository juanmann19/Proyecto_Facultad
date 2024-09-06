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
    public class AlumnoController : Controller
    {
        private readonly BdfflContext _context;

        public AlumnoController(BdfflContext context)
        {
            _context = context;
        }
        // GET: Alumno
        public async Task<IActionResult> Index()
        {
            return View(await _context.Alumnos.ToListAsync());
        }

        // GET: Alumno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(m => m.IdAlumno == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // GET: Alumno/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alumno/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAlumno,PrimerNombreAlumno,SegundoNombreAlumno,OtrosNombresAlumno,PrimerApellidoAlumno,SegundoApellidoAlumno,ApellidoCasado,FechaNacimientoAlumno,FechaBautizoAlumno,Direccion,Telefono,Dpi,Nit,EstadoCivil,Genero,NumeroCelula,CodigoFrater,Becado,Retiro,ReunionesEnConfianza,Otros,EstatusAlumno")] Alumno alumno)
        {
            //Validacion de que no existe un codigo frater
            var existingAlumno = await _context.Alumnos.FirstOrDefaultAsync(a =>
            a.CodigoFrater == alumno.CodigoFrater);

            if (existingAlumno != null)
            {
                // Agregar un error de validación si el código está duplicado
                ModelState.AddModelError("CodigoFrater", "El Código Frater ya está en uso.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        // GET: Alumno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            return View(alumno);
        }

        // POST: Alumno/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAlumno,PrimerNombreAlumno,SegundoNombreAlumno,OtrosNombresAlumno,PrimerApellidoAlumno,SegundoApellidoAlumno,ApellidoCasado,FechaNacimientoAlumno,FechaBautizoAlumno,Direccion,Telefono,Dpi,Nit,EstadoCivil,Genero,NumeroCelula,CodigoFrater,Becado,Retiro,ReunionesEnConfianza,Otros,EstatusAlumno")] Alumno alumno)
        {
            if (id != alumno.IdAlumno)
            {
                return NotFound();
            }

            var existingAlumno = await _context.Alumnos
            .FirstOrDefaultAsync(a => a.CodigoFrater == alumno.CodigoFrater && a.IdAlumno != alumno.IdAlumno); //valida que el id no exista, al mismo tiempo que el cod.frater no se repita en 2 usuarios sino solo 1

            if (existingAlumno != null)
            {
                // Agregar un error de validación si el código está duplicado
                ModelState.AddModelError("CodigoFrater", "El Código Frater ya está en uso.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnoExists(alumno.IdAlumno))
                    {
                        TempData["ErrorMessage"] = "El alumno no se encontró.";
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Se produjo un error al cambiar el estatus.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alumno);
        }

        // GET: Alumno/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var alumno = await _context.Alumnos
        //        .FirstOrDefaultAsync(m => m.IdAlumno == id);
        //    if (alumno == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(alumno);
        //}

        [HttpPost]
        public async Task<IActionResult> Deactivate(int? id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                TempData["ErrorMessage"] = "El alumno no se encontró.";
                return NotFound();
            }

            // Cambiar el estatus
            alumno.EstatusAlumno = !alumno.EstatusAlumno;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = alumno.EstatusAlumno ? "Alumno activado correctamente" : "Alumno desactivado correctamente";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(alumno.IdAlumno))
                {
                    TempData["ErrorMessage"] = "Error al actualizar el estatus del alumno.";
                    return NotFound();

                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
            //return View(alumno);

        }


        //// POST: Alumno/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var alumno = await _context.Alumnos.FindAsync(id);
        //    if (alumno != null)
        //    {
        //        _context.Alumnos.Remove(alumno);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool AlumnoExists(int id)
        {
            return _context.Alumnos.Any(e => e.IdAlumno == id);
        }

        public async Task<IActionResult> ReporteAlumnosInscritos()
        {
            return View(await _context.Alumnos.Where(a => a.EstatusAlumno).
                ToListAsync());
        }
    }
}
