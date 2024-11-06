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
       

        // GET: AsistenciaAlumnoes/Create
        public async Task<IActionResult> Create(int idAsistenciaStaff)
        {
            // Obtener el nombre del usuario autenticado.
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar al usuario basado en su nombre.
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el maestro basado en el IdUsuario.
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);
            if (staff == null)
            {
                return NotFound("No se encontró el maestro.");
            }

            // Obtener el registro de AsistenciaStaff
            var asistenciaStaff = await _context.AsistenciaStaffs
                .FirstOrDefaultAsync(a => a.IdAsistenciaStaff == idAsistenciaStaff && a.IdStaff == staff.IdStaff);

            if (asistenciaStaff == null)
            {
                return NotFound("No se encontró la asistencia del maestro para la mesa seleccionada.");
            }

            // Obtener los alumnos asignados a la mesa específica
            var alumnosAsignados = await _context.AsignacionAlumnos
                .Where(aa => aa.IdMesa == asistenciaStaff.IdMesa)
                .Include(a => a.IdAlumnoNavigation) // Incluir la información del alumno
                .ToListAsync();

            var modelo = new List<AsistenciaAlumno>();

            foreach (var asignacion in alumnosAsignados)
            {
                modelo.Add(new AsistenciaAlumno
                {
                    IdAlumno = asignacion.IdAlumno,
                    IdAlumnoNavigation = asignacion.IdAlumnoNavigation
                });
            }

            // Enviar el nombre del maestro y el IdAsistenciaStaff a la vista.
            ViewData["NombreMaestro"] = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}";
            ViewData["IdAsistenciaStaff"] = idAsistenciaStaff;

            return View(modelo);
        }

        // POST: AsistenciaAlumnoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<AsistenciaAlumno> asistencias, int idAsistenciaStaff)
        {
            // Obtener el nombre del usuario autenticado.
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar al usuario basado en su nombre.
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el maestro basado en el IdUsuario.
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);
            if (staff == null)
            {
                return NotFound("No se encontró el maestro.");
            }

            // Buscar el registro de AsistenciaStaff
            var asistenciaStaff = await _context.AsistenciaStaffs
                .FirstOrDefaultAsync(a => a.IdAsistenciaStaff == idAsistenciaStaff && a.IdStaff == staff.IdStaff);

            if (asistenciaStaff == null)
            {
                return NotFound("No se encontró la asistencia del maestro para la mesa seleccionada.");
            }

            // Guardar las asistencias de los alumnos.
            if (ModelState.IsValid)
            {
                // Obtener el último ID existente para evitar conflictos.
                var ultimaAsistenciaAlumno = await _context.AsistenciaAlumnos
                    .OrderByDescending(a => a.IdAsistenciaAlumno)
                    .Select(a => a.IdAsistenciaAlumno)
                    .FirstOrDefaultAsync();

                int nuevoIdAsistenciaAlumno = (ultimaAsistenciaAlumno != null) ? ultimaAsistenciaAlumno + 1 : 1;

                foreach (var asistencia in asistencias)
                {
                    // Asignar el IdAsistenciaStaff correcto.
                    asistencia.IdAsistenciaStaff = asistenciaStaff.IdAsistenciaStaff;

                    // Generar un nuevo ID para AsistenciaAlumno basado en el ID secuencial.
                    asistencia.IdAsistenciaAlumno = nuevoIdAsistenciaAlumno++;

                    // Verificar si la asistencia ya existe en el contexto y desconectarla.
                    var existingEntity = _context.AsistenciaAlumnos.Local.FirstOrDefault(a => a.IdAsistenciaAlumno == asistencia.IdAsistenciaAlumno);
                    if (existingEntity != null)
                    {
                        _context.Entry(existingEntity).State = EntityState.Detached; // Desconectar la entidad.
                    }

                    // Agregar la asistencia del alumno.
                    _context.AsistenciaAlumnos.Add(asistencia);
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asistencia registrada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, vuelve a la vista con los datos.
            // Reobtener la información necesaria para la vista.
            ViewData["NombreMaestro"] = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}";
            ViewData["IdAsistenciaStaff"] = idAsistenciaStaff;

            return View(asistencias);
        }

        // GET: AsistenciaAlumnoes/Index
        public async Task<IActionResult> Index()
        {
            // Obtener el nombre del usuario autenticado.
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar al usuario basado en su nombre.
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el maestro basado en el IdUsuario.
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);
            if (staff == null)
            {
                return NotFound("No se encontró el maestro.");
            }

            // Obtener las asistencias solo de los alumnos asignados a las mesas del maestro autenticado.
            var asistencias = await _context.AsistenciaAlumnos
                .Include(a => a.IdAlumnoNavigation)
                .Include(a => a.IdAsistenciaStaffNavigation)
                .Where(a => a.IdAsistenciaStaffNavigation.IdStaff == staff.IdStaff)
                .ToListAsync();

            return View(asistencias);
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
                    // Obtén el registro existente de la base de datos para asegurarte de que lo estás actualizando correctamente
                    var existingAsistencia = await _context.AsistenciaAlumnos.FindAsync(id);
                    if (existingAsistencia == null)
                    {
                        return NotFound();
                    }

                    // Actualiza solo el campo que quieres cambiar
                    existingAsistencia.Ausencia = asistenciaAlumno.Ausencia;

                    // Actualiza la entrada en el contexto
                    _context.AsistenciaAlumnos.Update(existingAsistencia);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "La asistencia del alumno se ha editado correctamente.";
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

