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
            var bdfflContext = _context.AsistenciaStaffs.Include(a => a.IdBimestreNavigation).Include(a => a.IdLeccionNavigation).Include(a => a.IdMesaNavigation).Include(a => a.IdStaffNavigation);
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

        // GET: AsistenciaStaffs/Create
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

            // Si todo va bien, continuamos con el código normal.
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
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");

            return View();
        }

        // POST: AsistenciaStaffs/Create
        // POST: AsistenciaStaffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaClase,IdMesa,IdLeccion,IdBimestre,Ausencia")] AsistenciaStaff asistenciaStaff)
        {
            // Ensure you set the IdStaff here based on the logged-in user
            var userName = User.Identity.Name;

            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == userName);
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.IdUsuario == usuario.IdUsuario);
            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Set the IdStaff in the AsistenciaStaff object
            asistenciaStaff.IdStaff = staff.IdStaff;

            var ultimaasistencia = _context.AsistenciaStaffs.OrderByDescending(a => a.IdAsistenciaStaff).FirstOrDefault();

            int nuevoId = (ultimaasistencia != null) ? ultimaasistencia.IdAsistenciaStaff + 1 : 1;

            asistenciaStaff.IdAsistenciaStaff = nuevoId;
            if (ModelState.IsValid)
            {
                _context.Add(asistenciaStaff);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asistencia creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            // Load data for dropdowns in case of errors
            await LoadViewData(asistenciaStaff.IdStaff); // Refactor this to load the necessary data

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
