using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using System.Security.Claims;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Coordinador, Admin")]
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
                 .Include(m => m.IdMesaNavigation.NombreSedeNavigation) 
                 .Include(m => m.IdMesaNavigation.IdJornadaNavigation)
                 .Include(m => m.IdMesaNavigation.AsignacionMaestros)
                 .ThenInclude(am => am.IdStaffNavigation)
                 .Select(a => new
                 {
                     a.IdAsignacionalumnos,
                     NombreCompletoAlumno = a.IdAlumnoNavigation.PrimerNombreAlumno + " " + a.IdAlumnoNavigation.PrimerApellidoAlumno,
                     Mesa = a.IdMesaNavigation.IdMesa,
                     Sede = a.IdMesaNavigation.NombreSedeNavigation.NombreSede, 
                     Jornada = $"{a.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {a.IdMesaNavigation.IdJornadaNavigation.Horario}",
                     NombreCompletoMaestro = a.IdMesaNavigation.AsignacionMaestros
                 .Select(am => $"{am.IdStaffNavigation.PrimerNombreStaff} {am.IdStaffNavigation.PrimerApellidoStaff}")
                 .FirstOrDefault() // Obtener el primer maestro asignado a la mesa
                 })
                .ToListAsync();

            return View(asignacionAlumnos);
        }


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
            // Obtener todas las mesas con los maestros asignados (si los hay), junto con la sede, el día de la semana y el horario.
            var mesasConMaestros = _context.Mesas
               .Select(m => new
               {
                   m.IdMesa,
                   MesaDescripcion = m.AsignacionMaestros
                       .Select(am => $"{am.IdStaffNavigation.PrimerNombreStaff} {am.IdStaffNavigation.PrimerApellidoStaff}")
                       .FirstOrDefault() + $" - {m.NombreSedeNavigation.NombreSede} - {m.IdJornadaNavigation.DiaSemana} {m.IdJornadaNavigation.Horario}"
               })
               .ToList();

            // Crear un SelectList con las mesas y la descripción de los maestros (si los hay).
            ViewData["IdMesa"] = new SelectList(mesasConMaestros, "IdMesa", "MesaDescripcion");

            // Obtener todos los alumnos con su nombre completo.
            var alumnos = _context.Alumnos
                .Select(a => new
                {
                    a.IdAlumno,
                    NombreCompleto = $"{a.PrimerNombreAlumno} {a.PrimerApellidoAlumno}"
                })
                .ToList();

            // Verificar si la lista de alumnos no está vacía.
            if (!alumnos.Any())
            {
                // Manejar el caso donde no hay alumnos.
                ViewBag.IdAlumno = new SelectList(Enumerable.Empty<SelectListItem>(), "IdAlumno", "NombreCompleto");
            }
            else
            {
                // Crear el SelectList para los alumnos.
                ViewBag.IdAlumno = new SelectList(alumnos, "IdAlumno", "NombreCompleto");
            }

            return View();
        }

        //    // Aquí obtienes el nombre del usuario autenticado.
        //    var userName = User.Identity.Name;

        //    // Verificar si obtenemos el nombre de usuario autenticado.
        //    if (string.IsNullOrEmpty(userName))
        //    {
        //        return Unauthorized("Usuario no autenticado.");
        //    }

        //    // Buscar el usuario basado en su nombre.
        //    var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);

        //    // Verificar si el usuario existe en la tabla Usuarios.
        //    if (usuario == null)
        //    {
        //        return NotFound("Usuario no encontrado.");
        //    }

        //    // Ahora, usa el IdUsuario para buscar el staff (maestro).
        //    var staff = _context.Staff.FirstOrDefault(s => s.IdUsuario == usuario.IdUsuario);

        //    // Verificar si el staff existe en la tabla Staff.
        //    if (staff == null)
        //    {
        //        return NotFound("No se encontró el maestro relacionado con el usuario.");
        //    }

        //    // Obtener mesas asignadas al maestro
        //    var mesasAsignadas = _context.AsignacionMaestros
        //        .Where(am => am.IdStaff == staff.IdStaff)
        //        .Select(am => new
        //        {
        //            am.IdMesa,
        //            MesaDescripcion = $"{am.IdMesaNavigation.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {am.IdMesaNavigation.IdJornadaNavigation.Horario}",
        //            NombreCompletoStaff = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}"
        //        })
        //        .ToList();

        //    // Crear un SelectList con el nombre completo del staff y la descripción de las mesas asignadas.
        //    ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion", "NombreCompletoStaff");

        //    // Enviar el nombre del maestro a la vista.
        //    ViewData["NombreMaestro"] = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}";

        //    // Crear un SelectList con las mesas asignadas.
        //    ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion");

        //    // Obtener todos los alumnos con su nombre completo
        //    var alumnos = _context.Alumnos
        //        .Select(a => new
        //        {
        //            a.IdAlumno,
        //            NombreCompleto = $"{a.PrimerNombreAlumno} {a.PrimerApellidoAlumno}"
        //        })
        //        .ToList();

        //    // Verifica si la lista de alumnos no está vacía
        //    if (alumnos == null || !alumnos.Any())
        //    {
        //        // Manejar el caso donde no hay alumnos
        //        ViewBag.IdAlumno = new SelectList(Enumerable.Empty<SelectListItem>(), "IdAlumno", "NombreCompleto");
        //    }
        //    else
        //    {
        //        ViewBag.IdAlumno = new SelectList(alumnos, "IdAlumno", "NombreCompleto");
        //    }

        //    // Obtener la lista de staff
        //    ViewBag.IdStaff = new SelectList(_context.Mesas, "IdMesa", "IdMesa");

        //    return View();
        // }


        // POST: AsignacionAlumno/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: AsignacionAlumno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsignacionalumnos,IdAlumno,IdMesa")] AsignacionAlumno asignacionAlumno)
        {
            // Verifica si el modelo es válido antes de continuar
            if (!ModelState.IsValid)
            {
                await LoadViewBagData(); // Cargar datos para las listas
                return View(asignacionAlumno);
            }

            // Verificar si existen datos en Alumnos y Mesas
            var alumnos = await _context.Alumnos.ToListAsync();
            var mesas = await _context.Mesas.ToListAsync();

            // Comprobar si hay alumnos o mesas
            if (!alumnos.Any() || !mesas.Any())
            {
                TempData["ErrorMessage"] = "No se encontraron alumnos o mesas.";
                return RedirectToAction("Index"); // Redirigir a Index
            }

            // Llenar las listas de selección
            ViewBag.IdAlumno = new SelectList(alumnos, "IdAlumno", "NombreCompleto");
            ViewBag.IdMesa = new SelectList(mesas, "IdMesa", "MesaDescripcion");

            // Verificar si el alumno ya está asignado a una mesa
            var existingAssignment = await _context.AsignacionAlumnos
                .FirstOrDefaultAsync(a => a.IdAlumno == asignacionAlumno.IdAlumno);

            if (existingAssignment != null)
            {
                TempData["ErrorMessage"] = "Este alumno ya está asignado a una mesa.";
                return RedirectToAction("Index"); // Redirigir a Index
            }

            // Guardar la nueva asignación
            _context.AsignacionAlumnos.Add(asignacionAlumno);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Alumno asignado a la mesa exitosamente.";

            return RedirectToAction("Index");
        }
        private async Task LoadViewBagData()
        {
            var alumnos = await _context.Alumnos
                .Select(a => new
                {
                    a.IdAlumno,
                    NombreCompleto = $"{a.PrimerNombreAlumno} {a.PrimerApellidoAlumno}"
                })
                .ToListAsync();

            var mesas = await _context.Mesas
                .Select(m => new
                {
                    m.IdMesa,
                    MesaDescripcion = m.AsignacionMaestros
                        .Select(am => $"{am.IdStaffNavigation.PrimerNombreStaff} {am.IdStaffNavigation.PrimerApellidoStaff}")
                        .FirstOrDefault() + $" - {m.NombreSedeNavigation.NombreSede} - {m.IdJornadaNavigation.DiaSemana} {m.IdJornadaNavigation.Horario}"
                })
                .ToListAsync();

            ViewBag.IdAlumno = new SelectList(alumnos, "IdAlumno", "NombreCompleto");
            ViewBag.IdMesa = new SelectList(mesas, "IdMesa", "MesaDescripcion");
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

            await LoadViewBagData(); // Cargar datos para los SelectList

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
