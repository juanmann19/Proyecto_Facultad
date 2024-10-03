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
    [Authorize(Roles = "Maestro, Auxiliar, Admin")]
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
            var asistencias = await _context.AsistenciaAlumnos
                .Include(a => a.IdAlumnoNavigation)
                .Include(a => a.IdAsistenciaStaffNavigation)
                .ToListAsync();

            return View(asistencias);
        }

        // GET: AsistenciaAlumnoes/Create
        public IActionResult Create()
        {
            // Aquí obtienes el nombre del usuario autenticado
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar al usuario basado en su nombre
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el maestro basado en el IdUsuario
            var staff = _context.Staff.FirstOrDefault(s => s.IdUsuario == usuario.IdUsuario);

            if (staff == null)
            {
                return NotFound("No se encontró el maestro.");
            }

            // Buscar los alumnos asignados a la mesa del maestro
            var alumnosAsignados = _context.AsignacionMaestros
                .Where(am => am.IdStaff == staff.IdStaff)
                .SelectMany(am => am.IdMesaNavigation.AsignacionAlumnos)
                .Select(aa => new AsistenciaAlumno
                {
                    IdAlumno = aa.IdAlumno,
                    IdAlumnoNavigation = aa.IdAlumnoNavigation 
                })
                .ToList();

            // Crear la vista con la lista de alumnos asignados
            ViewBag.Alumnos = alumnosAsignados;
            ViewData["NombreMaestro"] = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}";

            // Pasar la lista de alumnos como modelo a la vista
            return View(alumnosAsignados);
        }

        // POST: AsistenciaAlumnoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<AsistenciaAlumno> asistenciaAlumnos)
        {
            if (ModelState.IsValid)
            {
                // Obtener el ID del staff desde el usuario autenticado
                var userName = User.Identity.Name;
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);
                var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);

                // Configurar cada asistencia alumno
                foreach (var asistenciaAlumno in asistenciaAlumnos)
                {
                    asistenciaAlumno.IdAsistenciaStaff = staff.IdStaff;

                    // Generar un nuevo Id para cada registro de asistencia alumno
                    var ultimaAsistencia = await _context.AsistenciaAlumnos
                        .OrderByDescending(a => a.IdAsistenciaAlumno)
                        .FirstOrDefaultAsync();

                    // Asignar nuevo Id
                    asistenciaAlumno.IdAsistenciaAlumno = (ultimaAsistencia != null) ? ultimaAsistencia.IdAsistenciaAlumno + 1 : 1;

                    // Agregar la asistencia al contexto
                    await _context.AsistenciaAlumnos.AddAsync(asistenciaAlumno);

                    // Guardar cambios en la base de datos para la asistencia actual
                    await _context.SaveChangesAsync();

                    // Mostrar mensaje de éxito para la asistencia actual
                    TempData["SuccessMessage"] = "Asistencia guardada correctamente.";
                    // Esperar un breve período para mostrar el mensaje
                    await Task.Delay(2000); // Espera 2 segundos (puedes ajustar el tiempo según sea necesario)
                }

                return RedirectToAction(nameof(Index));
            }

            // Recargar los datos en caso de error
            return View(asistenciaAlumnos);
        }


        // GET: AsistenciaAlumnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaAlumno = await _context.AsistenciaAlumnos
                .Include(a => a.IdAlumnoNavigation) // Incluye la navegación al alumno
                .FirstOrDefaultAsync(a => a.IdAsistenciaAlumno == id);

            if (asistenciaAlumno == null)
            {
                return NotFound();
            }
            return View(asistenciaAlumno);
        }

        // POST: AsistenciaAlumnoes/Edit/5
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
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaAlumnoExists(asistenciaAlumno.IdAsistenciaAlumno))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la asistencia. Detalles: " + ex.Message);
                }
            }

            // Si llegamos aquí, algo falló, vuelve a mostrar el formulario
            return View(asistenciaAlumno);
        }

        private bool AsistenciaAlumnoExists(int id)
        {
            return _context.AsistenciaAlumnos.Any(e => e.IdAsistenciaAlumno == id);
        }
        
    }
}
