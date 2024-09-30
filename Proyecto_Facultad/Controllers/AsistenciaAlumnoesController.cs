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

        // GET: AsistenciaAlumnos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaAlumno = await _context.AsistenciaAlumnos
                .Include(a => a.IdAlumnoNavigation)        // Incluir datos del alumno
                .Include(a => a.IdAsistenciaStaffNavigation) // Incluir datos del staff de asistencia
                .FirstOrDefaultAsync(m => m.IdAsistenciaAlumno == id); // Buscar por IdAsistenciaAlumno

            if (asistenciaAlumno == null)
            {
                return NotFound();
            }

            return View(asistenciaAlumno);
        }

        // GET: AsistenciaAlumnoes/Create
        public async Task<IActionResult> Create()
        {
            // Aquí obtienes el nombre del usuario autenticado.
            var userName = User.Identity.Name;

            // Verificar si obtenemos el nombre de usuario autenticado.
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar el usuario basado en su nombre de forma asincrónica.
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);

            // Verificar si el usuario existe en la tabla Usuarios.
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Ahora, usa el IdUsuario para buscar el staff (maestro) de forma asincrónica.
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);

            // Verificar si el staff existe en la tabla Staff.
            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Obtener mesas asignadas al maestro de forma asincrónica
            var mesasAsignadas = await _context.AsignacionMaestros
                .Where(am => am.IdStaff == staff.IdStaff)
                .Select(am => new
                {
                    am.IdMesa,
                    MesaDescripcion = $"{am.IdMesaNavigation.IdMesa}",

                    // Obtener alumnos asignados a cada mesa
                    Alumnos = _context.AsignacionAlumnos
                        .Where(aa => aa.IdMesa == am.IdMesa)
                        .Select(aa => new
                        {
                            aa.IdAlumnoNavigation.IdAlumno,
                            NombreCompleto = $"{aa.IdAlumnoNavigation.PrimerNombreAlumno} {aa.IdAlumnoNavigation.PrimerApellidoAlumno}",
                        }).ToList()
                })
                .ToListAsync();

            // Combinar todos los alumnos en una sola lista
            var listaAlumnos = mesasAsignadas.SelectMany(m => m.Alumnos)
                .Distinct() // Para evitar duplicados si un alumno está en varias mesas
                .ToList();

            // Pasar la lista de alumnos a la vista
            ViewBag.Alumnos = listaAlumnos;

            // Pasar el nombre del maestro a la vista
            ViewData["NombreMaestro"] = $"{staff.PrimerNombreStaff} {staff.PrimerApellidoStaff}";

            // Crear un SelectList con las mesas asignadas.
            ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion");

            // Otros datos necesarios para el formulario.
            ViewData["IdAlumno"] = new SelectList(_context.Alumnos, "IdAlumno", "PrimerNombreAlumno");
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");

            return View();
        }
        // POST: AsistenciaAlumnoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<AsistenciaAlumno> asistenciaAlumnos)
        {
            if (asistenciaAlumnos == null || !asistenciaAlumnos.Any())
            {
                return BadRequest("La lista de asistencia no puede ser nula o vacía.");
            }

            foreach (var asistencia in asistenciaAlumnos)
            {
                // Validar que el alumno esté seleccionado
                if (asistencia.IdAlumno == 0)
                {
                    return BadRequest("Falta la información del alumno.");
                }

                // Verificar si ya existe una asistencia para el alumno y el maestro
                var existingAsistencia = await _context.AsistenciaAlumnos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.IdAlumno == asistencia.IdAlumno && a.IdAsistenciaStaff == asistencia.IdAsistenciaStaff);

                // Si no existe, se agrega una nueva
                if (existingAsistencia == null)
                {
                    _context.AsistenciaAlumnos.Add(asistencia);
                }
                else
                {
                    // Si existe, se puede actualizar solo si el estado de asistencia es "Ausente"
                    if (asistencia.Ausencia.Contains("Ausente"))
                    {
                        existingAsistencia.Ausencia = asistencia.Ausencia; // Actualiza solo si el alumno está ausente
                        _context.AsistenciaAlumnos.Update(existingAsistencia);
                    }
                }
            }

            // Guardar todos los cambios al contexto
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Asistencias guardadas correctamente.";
            return RedirectToAction("Index");
        }


        // POST: AsistenciaAlumnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //[ValidateAntiForgeryToken]
        //[HttpPost]
        //public IActionResult Create (List<AsistenciaAlumno> asistenciaAlumnos, DateTime FechaAsistencia)
        //{
        //    if (asistenciaAlumnos == null || asistenciaAlumnos.Count == 0)
        //    {
        //        return BadRequest("No se ha recibido información de asistencia.");
        //    }

        //    // Guardar cada asistencia de alumno
        //    foreach (var asistencia in asistenciaAlumnos)
        //    {
        //        var asistenciaAlumno = new AsistenciaAlumno
        //        {
        //            IdAlumno = asistencia.IdAlumno,

        //            Ausencia = asistencia.Ausencia,

        //        };

        //        _context.AsistenciaAlumnos.Add(asistenciaAlumno);
        //    }

        //    _context.SaveChanges();
        //    return RedirectToAction("Index");

        private bool AsistenciaAlumnoExists(int id)
        {
            return _context.AsistenciaAlumnos.Any(e => e.IdAsistenciaAlumno == id);
        }
    }
}


