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

    public class CapacitacionController : Controller
    {
        private readonly BdfflContext _context;

        public CapacitacionController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Capacitacion
        [Authorize (Roles = "Coordinador, Auxiliar, Admin")]
        public async Task<IActionResult> Index()
        {
            // Incluye la navegación a Staff
            var bdfflContext = _context.Capacitacions.Include(c => c.IdStaffNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: Capacitacion/Details/5
        [Authorize (Roles = "Coordinador, Auxiliar, Admin")]
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

        // GET: Capacitacion/Create
        [Authorize (Roles = "Coordinador, Auxiliar, Admin")]
        public IActionResult Create()
        {
            // Obtener los nombres completos de los maestros (staff).
            var staffList = _context.Staff
                .Select(s => new
                {
                    s.IdStaff,
                    NombreCompleto = $"{s.PrimerNombreStaff} {s.PrimerApellidoStaff}"
                })
                .ToList();

            // Crear el SelectList para los maestros, utilizando el nombre completo.
            ViewData["IdStaff"] = new SelectList(staffList, "IdStaff", "NombreCompleto");

            return View();
        }


        // POST: Capacitacion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Coordinador, Auxiliar, Admin")]
        public async Task<IActionResult> Create([Bind("IdCapacitacion,FechaCapacitacion,IdStaff")] Capacitacion capacitacion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(capacitacion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Dato cargado correctamente";
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
                }
            }
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "PrimerNombreStaff", capacitacion.IdStaff);
            return View(capacitacion);
        }

        // GET: Capacitacion/Edit/5
        [Authorize (Roles = "Coordinador, Auxiliar, Admin")]
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

            // Obtener los nombres completos de los maestros (staff).
            var staffList = _context.Staff
                .Select(s => new
                {
                    s.IdStaff,
                    NombreCompleto = $"{s.PrimerNombreStaff} {s.PrimerApellidoStaff}"
                })
                .ToList();

            // Crear el SelectList para los maestros, utilizando el nombre completo.
            ViewData["IdStaff"] = new SelectList(staffList, "IdStaff", "NombreCompleto", capacitacion.IdStaff);

            return View(capacitacion);
        }


        // POST: Capacitacion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Coordinador, Auxiliar, Admin")]
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
                    TempData["SuccessMessage"] = "Datos actualizados correctamente";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapacitacionExists(capacitacion.IdCapacitacion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Se produjo un error al actualizar los datos.";
                        throw;
                    }
                }
            }

            // En caso de error, vuelve a cargar el SelectList con los nombres completos.
            var staffList = _context.Staff
                .Select(s => new
                {
                    s.IdStaff,
                    NombreCompleto = $"{s.PrimerNombreStaff} {s.PrimerApellidoStaff}"
                })
                .ToList();
            ViewData["IdStaff"] = new SelectList(staffList, "IdStaff", "NombreCompleto", capacitacion.IdStaff);

            return View(capacitacion);
        }

        // GET: Capacitacion/Delete/5
        [Authorize(Roles = "Coordinador, Auxiliar, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var capacitacion = await _context.Capacitacions
                .Include(c => c.IdStaffNavigation) // Incluir los datos del maestro
                .FirstOrDefaultAsync(m => m.IdCapacitacion == id);

            if (capacitacion == null)
            {
                return NotFound();
            }

            // Concatenar el nombre completo del maestro en la vista.
            ViewBag.NombreCompletoStaff = $"{capacitacion.IdStaffNavigation.PrimerNombreStaff} {capacitacion.IdStaffNavigation.PrimerApellidoStaff}";

            return View(capacitacion);
        }

        // POST: Capacitacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinador, Auxiliar, Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var capacitacion = await _context.Capacitacions
                .Include(c => c.IdStaffNavigation) // Incluir nuevamente la navegación a Staff para obtener el nombre completo
                .FirstOrDefaultAsync(c => c.IdCapacitacion == id);

            if (capacitacion != null)
            {
                // Guardar el nombre completo para mostrar en el mensaje de confirmación
                var nombreCompletoStaff = $"{capacitacion.IdStaffNavigation.PrimerNombreStaff} {capacitacion.IdStaffNavigation.PrimerApellidoStaff}";

                _context.Capacitacions.Remove(capacitacion);
                await _context.SaveChangesAsync();

                // Usar el nombre completo en el mensaje de éxito
                TempData["SuccessMessage"] = $"Capacitación con el maestro {nombreCompletoStaff} eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Se produjo un error al intentar eliminar la capacitación.";
            }

            return RedirectToAction(nameof(Index));
        }


        // Acción para el reporte de capacitaciones
        [Authorize (Roles = "Coordinador, Admin")]
        public async Task<IActionResult> Reporte(string nombreStaff, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var capacitacionesQuery = _context.Capacitacions
                .Include(c => c.IdStaffNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombreStaff))
            {
                capacitacionesQuery = capacitacionesQuery
                    .Where(c => EF.Functions.Like(c.IdStaffNavigation.PrimerNombreStaff, $"%{nombreStaff}%"));
            }

            if (fechaInicio.HasValue)
            {
                capacitacionesQuery = capacitacionesQuery.Where(c => c.FechaCapacitacion >= fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                capacitacionesQuery = capacitacionesQuery.Where(c => c.FechaCapacitacion <= fechaFin.Value);
            }

            var capacitaciones = await capacitacionesQuery.ToListAsync();

            // Agrupar por mes
            var capacitacionesPorMes = capacitaciones
                .GroupBy(c => c.FechaCapacitacion.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .ToList();

            // Obtener los nombres de los maestros asociados
            var capacitacionesConNombresPorMes = capacitaciones
                .GroupBy(c => c.FechaCapacitacion.Month)
                .Select(g => new
                {
                    NombresMaestros = g.Select(c => c.IdStaffNavigation.PrimerNombreStaff).Distinct().ToList()
                })
                .ToList();

            // Pasar los datos a la vista usando ViewBag
            ViewBag.NombreStaff = nombreStaff;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.CapacitacionesPorMes = capacitacionesPorMes;
            ViewBag.CapacitacionesConNombresPorMes = capacitacionesConNombresPorMes;

            return View();
        }

        //public async Task<IActionResult> Reporte(string nombreStaff, DateTime? fechaInicio, DateTime? fechaFin)
        //{
        //    // Filtrar capacitaciones basadas en los parámetros de búsqueda
        //    var capacitacionesQuery = _context.Capacitacions
        //        .Include(c => c.IdStaffNavigation)
        //        .AsQueryable();

        //    if (!string.IsNullOrEmpty(nombreStaff))
        //    {
        //        capacitacionesQuery = capacitacionesQuery
        //            .Where(c => c.IdStaffNavigation.PrimerNombreStaff.Contains(nombreStaff, StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (fechaInicio.HasValue)
        //    {
        //        capacitacionesQuery = capacitacionesQuery.Where(c => c.FechaCapacitacion >= fechaInicio.Value);
        //    }

        //    if (fechaFin.HasValue)
        //    {
        //        capacitacionesQuery = capacitacionesQuery.Where(c => c.FechaCapacitacion <= fechaFin.Value);
        //    }


        //    var capacitaciones = await capacitacionesQuery.ToListAsync();

        //    // Agrupar por mes
        //    var capacitacionesPorMes = capacitaciones
        //        .GroupBy(c => c.FechaCapacitacion.Month)
        //        .Select(g => new
        //        {
        //            Mes = g.Key,
        //            Total = g.Count()
        //        })
        //        .ToList();

        //    // Obtener los nombres de los maestros asociados
        //    var capacitacionesConNombresPorMes = capacitaciones
        //        .GroupBy(c => c.FechaCapacitacion.Month)
        //        .Select(g => new
        //        {
        //            NombresMaestros = g.Select(c => c.IdStaffNavigation.PrimerNombreStaff).Distinct().ToList()
        //        })
        //        .ToList();

        //    // Pasar los datos a la vista usando ViewBag
        //    ViewBag.NombreStaff = nombreStaff;
        //    ViewBag.FechaInicio = fechaInicio;
        //    ViewBag.FechaFin = fechaFin;
        //    ViewBag.CapacitacionesPorMes = capacitacionesPorMes;
        //    ViewBag.CapacitacionesConNombresPorMes = capacitacionesConNombresPorMes;

        //    return View();
        //}

        private bool CapacitacionExists(int id)
        {
            return _context.Capacitacions.Any(e => e.IdCapacitacion == id);
        }
    }
}

