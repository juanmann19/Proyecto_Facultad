using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Maestro, Auxiliar, Admin")]
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
            // Aquí obtienes el nombre del usuario autenticado.
            var userName = User.Identity.Name;

            // Verificar si obtenemos el nombre de usuario autenticado.
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar el usuario basado en su nombre.
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);

            // Verificar si el usuario existe en la tabla Usuarios.
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Ahora, usa el IdUsuario para buscar el staff (maestro).
            var staff = _context.Staff.FirstOrDefault(s => s.IdUsuario == usuario.IdUsuario);

            // Verificar si el staff existe en la tabla Staff.
            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Obtener mesas asignadas al maestro
            var mesasAsignadas = _context.AsignacionMaestros
                .Where(am => am.IdStaff == staff.IdStaff)
                .Select(am => new
                {
                    am.IdMesa,
                    MesaDescripcion = $"{am.IdMesaNavigation.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {am.IdMesaNavigation.IdJornadaNavigation.Horario}"
                })
                .ToList();

            // Enviar el nombre del maestro a la vista.
            ViewData["NombreMaestro"] = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}";

            // Crear un SelectList con las mesas asignadas.
            ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion");

            // Otros datos necesarios para el formulario.
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno");
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");

            return View();
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
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", asistenciaAlumno.IdAlumno);
            ViewData["IdAsistenciaStaff"] = new SelectList(_context.AsistenciaStaffs, "IdAsistenciaStaff", "IdStaff", asistenciaAlumno.IdAsistenciaStaff);
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
                    TempData["ErrorMessage"] = "Datos actualizados correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaAlumnoExists(asistenciaAlumno.IdAsistenciaAlumno))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Se produjo un error al actualizar datos.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno", asistenciaAlumno.IdAlumno);
            ViewData["IdAsistenciaStaff"] = new SelectList(_context.AsistenciaStaffs, "IdAsistenciaStaff", "IdStaff", asistenciaAlumno.IdAsistenciaStaff);
            return View(asistenciaAlumno);
        }
        private bool AsistenciaAlumnoExists(int id)
        {
            return _context.AsistenciaAlumnos.Any(e => e.IdAsistenciaAlumno == id);
        }
    }
}