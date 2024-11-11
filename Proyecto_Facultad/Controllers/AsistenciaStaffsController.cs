using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Facultad.Models;
using System.Security.Claims;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Maestro, Auxiliar, Admin")]

    public class AsistenciaStaffsController : Controller
    {
        private readonly BdfflContext _context;

        public AsistenciaStaffsController(BdfflContext context)
        {
            _context = context;
        }
        // GET: AsistenciaStaffs
        public async Task<IActionResult> Index()
        {
            // Obtener el nombre del usuario autenticado
            var userName = User.Identity.Name;

            // Verificar si obtenemos el nombre de usuario autenticado
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar el usuario basado en su nombre
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);

            // Verificar si el usuario existe en la tabla Usuarios
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Ahora, usa el IdUsuario para buscar el staff (maestro)
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);

            // Verificar si el staff existe en la tabla Staff
            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Filtrar las asistencias del staff autenticado y ordenar por fecha descendente
            var bdfflContext = _context.AsistenciaStaffs
                .Where(a => a.IdStaff == staff.IdStaff)  // Solo las asistencias del staff actual
                .OrderByDescending(a => a.FechaClase)    // Ordenar por FechaClase en orden descendente
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdStaffNavigation);

            return View(await bdfflContext.ToListAsync());
        }


        // GET: AsistenciaStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaStaff = await _context.AsistenciaStaffs
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdAsistenciaStaff == id);
            if (asistenciaStaff == null)
            {
                return NotFound();
            }

            return View(asistenciaStaff);
        }
        [HttpGet]
        public IActionResult GetMesasPorMaestro(int idStaff)
{
             // Consulta que obtiene las mesas asignadas al maestro seleccionado
             var mesas = _context.AsignacionMaestros
                        .Where(a => a.IdStaff == idStaff) // Filtra las asignaciones por el Id del maestro
                        .Select(a => new {
                            idMesa = a.IdMesaNavigation.IdMesa, // Obtén el ID de la mesa
                            mesaDescripcion = $"Mesa {a.IdMesaNavigation.IdMesa} - Jornada: {a.IdMesaNavigation.IdJornadaNavigation.DiaSemana} - {a.IdMesaNavigation.IdJornadaNavigation.Horario}" // Usa DiaSemana y Horario
                        })
                        .ToList();

             return Json(mesas); // Devolver las mesas como JSON
        }

        // GET: AsistenciaStaffs/Create
        public IActionResult Create()
        {
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Obtener el usuario autenticado y su información
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == userName);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Obtener el staff (maestro) del usuario autenticado
            var staffActual = _context.Staff.FirstOrDefault(s => s.IdUsuario == usuario.IdUsuario);
            if (staffActual == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Pasar el nombre del maestro autenticado a la vista
            ViewData["NombreMaestroActual"] = $"{staffActual.PrimerNombreStaff} {staffActual.PrimerApellidoStaff}";

            // Cargar todos los maestros en el sistema, marcando al maestro actual como seleccionado por defecto
            var maestros = _context.Staff.Select(s => new SelectListItem
            {
                Value = s.IdStaff.ToString(),
                Text = $"{s.PrimerNombreStaff} {s.PrimerApellidoStaff}",
                Selected = s.IdStaff == staffActual.IdStaff // Seleccionar por defecto el maestro autenticado
            }).ToList();

            // Cargar las mesas asignadas al maestro autenticado por defecto
            var mesasAsignadas = _context.AsignacionMaestros
                .Where(am => am.IdStaff == staffActual.IdStaff)
                .Select(am => new SelectListItem
                {
                    Value = am.IdMesa.ToString(),
                    Text = $"{am.IdMesaNavigation.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {am.IdMesaNavigation.IdJornadaNavigation.Horario}"
                })
                .ToList();

            // Pasar las listas de maestros y mesas a la vista
            ViewData["IdStaff"] = new SelectList(maestros, "Value", "Text", staffActual.IdStaff);
            ViewData["IdMesa"] = new SelectList(mesasAsignadas, "Value", "Text");

            // Otras listas desplegables
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");

            return View();
        }


        // POST: AsistenciaStaffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaClase,IdMesa,IdLeccion,IdBimestre,Ausencia")] AsistenciaStaff asistenciaStaff)
        {
            // Obtener el nombre del usuario autenticado.
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar el usuario basado en su nombre.
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el staff (maestro) basado en el IdUsuario.
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);
            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Asignar el IdStaff al objeto AsistenciaStaff.
            asistenciaStaff.IdStaff = staff.IdStaff;

            // Generar un nuevo IdAsistenciaStaff.
            var ultimaAsistencia = await _context.AsistenciaStaffs.OrderByDescending(a => a.IdAsistenciaStaff).FirstOrDefaultAsync();
            asistenciaStaff.IdAsistenciaStaff = (ultimaAsistencia != null) ? ultimaAsistencia.IdAsistenciaStaff + 1 : 1;

            if (ModelState.IsValid)
            {
                // Guardar la asistencia del maestro.
                _context.AsistenciaStaffs.Add(asistenciaStaff);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Asistencia creada correctamente.";

                // Redirigir al método Create del controlador AsistenciaAlumnoes, pasando el IdAsistenciaStaff.
                return RedirectToAction("Create", "AsistenciaAlumnoes", new { idAsistenciaStaff = asistenciaStaff.IdAsistenciaStaff });
            }
            return View(asistenciaStaff);
        }

        private async Task LoadViewData(int staffId)
        {
            var mesasAsignadas = await _context.AsignacionMaestros
                .Where(am => am.IdStaff == staffId)
                .Select(am => new
                {
                    am.IdMesa,
                    MesaDescripcion = $"{am.IdMesaNavigation.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {am.IdMesaNavigation.IdJornadaNavigation.Horario}"
                })
                .ToListAsync();

            ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion");
            ViewData["IdLeccion"] = new SelectList(await _context.Leccions.ToListAsync(), "IdLeccion", "Descripcion");
            ViewData["IdBimestre"] = new SelectList(await _context.Bimestres.ToListAsync(), "IdBimestre", "NombreBimestre");
        }
    

        // GET: AsistenciaStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaStaff = await _context.AsistenciaStaffs
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdStaffNavigation) // Incluye la navegación de Staff
                .FirstOrDefaultAsync(a => a.IdAsistenciaStaff == id);

            if (asistenciaStaff == null)
            {
                return NotFound();
            }

            // Cargar las mesas asignadas al Staff de la asistencia
            var mesasAsignadas = _context.AsignacionMaestros
                .Where(am => am.IdStaff == asistenciaStaff.IdStaff)
                .Include(am => am.IdMesaNavigation)
                .Include(am => am.IdStaffNavigation)
                .Select(am => new
                {
                    am.IdMesa,
                    MesaDescripcion = $"{am.IdMesaNavigation.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {am.IdMesaNavigation.IdJornadaNavigation.Horario}",
                    NombreCompletoMaestro = $"{am.IdStaffNavigation.PrimerNombreStaff} {am.IdStaffNavigation.PrimerApellidoStaff}"
                })
                .ToList();

            // Obtener el nombre del maestro para mostrarlo en la vista
            var maestro = mesasAsignadas.FirstOrDefault()?.NombreCompletoMaestro;

            if (maestro != null)
            {
                ViewData["NombreMaestro"] = maestro;
            }

            // Cargar las listas desplegables
            ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion", asistenciaStaff.IdMesa);
            ViewData["IdLeccion"] = new SelectList(await _context.Leccions.ToListAsync(), "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdBimestre"] = new SelectList(await _context.Bimestres.ToListAsync(), "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);
             // Incluye la lista de Staff

            return View(asistenciaStaff);
        }

        // POST: AsistenciaStaffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAsistenciaStaff,IdStaff,FechaClase,IdMesa,IdLeccion,IdBimestre,Ausencia")] AsistenciaStaff asistenciaStaff)
        {
            if (id != asistenciaStaff.IdAsistenciaStaff)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(asistenciaStaff); // No debes modificar IdAsistenciaStaff
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Asistencia guardada correctamente.";
                 
                    // Obtener la entidad existente desde la base de datos
                  
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaStaffExists(asistenciaStaff.IdAsistenciaStaff))
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

            // Cargar valores en caso de error
            var mesasAsignadas = _context.AsignacionMaestros
                .Where(am => am.IdStaff == asistenciaStaff.IdStaff)
                .Select(am => new
                {
                    am.IdMesa,
                    MesaDescripcion = $"{am.IdMesaNavigation.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana}"
                })
                .ToList();

            ViewData["IdMesa"] = new SelectList(mesasAsignadas, "IdMesa", "MesaDescripcion", asistenciaStaff.IdMesa);
            ViewData["IdLeccion"] = new SelectList(await _context.Leccions.ToListAsync(), "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdBimestre"] = new SelectList(await _context.Bimestres.ToListAsync(), "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);

            return View(asistenciaStaff);
        }



        // GET: AsistenciaStaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaStaff = await _context.AsistenciaStaffs
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdStaffNavigation)
                .FirstOrDefaultAsync(m => m.IdAsistenciaStaff == id);
            if (asistenciaStaff == null)
            {
                return NotFound();
            }

            return View(asistenciaStaff);
        }

        // POST: AsistenciaStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var asistenciaStaff = await _context.AsistenciaStaffs.FindAsync(id);
            if (asistenciaStaff != null)
            {
                _context.AsistenciaStaffs.Remove(asistenciaStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AsistenciaStaffExists(int id)
        {
            return _context.AsistenciaStaffs.Any(e => e.IdAsistenciaStaff == id);
        }
    }
}
