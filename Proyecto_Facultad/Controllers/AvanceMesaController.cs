using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Proyecto_Facultad.Controllers
{
    [Authorize(Roles = "Maestro, Auxiliar, Admin")]
    public class AvanceMesaController : Controller
    {
        private readonly BdfflContext _context;

        public AvanceMesaController(BdfflContext context)
        {
            _context = context;
        }

        // GET: AvanceMesa
        public async Task<IActionResult> Index()
        {
            // Obtener el nombre de usuario autenticado
            var nombreUsuario = User.Identity.Name;

            // Verificar si el nombre de usuario está vacío
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar el usuario en la tabla Usuarios basado en el nombre de usuario
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el staff (maestro) asociado al usuario autenticado
            var staff = _context.Staff.FirstOrDefault(s => s.IdUsuario == usuario.IdUsuario);

            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Obtener los IDs de las mesas asignadas al maestro
            var mesasAsignadasIds = _context.AsignacionMaestros
                .Where(am => am.IdStaff == staff.IdStaff)
                .Select(am => am.IdMesa)
                .ToList();

            // Filtrar los avances de mesa para mostrar solo los de las mesas asignadas
            var avances = _context.AvanceMesas
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdLibroNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdNivelNavigation)
                .Where(a => mesasAsignadasIds.Contains(a.IdMesa)); // Filtrar por mesas asignadas

            return View(await avances.ToListAsync());
        }


        // GET: AvanceMesa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avanceMesa = await _context.AvanceMesas
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdLibroNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdNivelNavigation)
                .FirstOrDefaultAsync(m => m.IdAvancemesa == id);

            if (avanceMesa == null)
            {
                return NotFound();
            }

            return View(avanceMesa);
        }

        // GET: AvanceMesa/Create
        public IActionResult Create()
        {
            // Obtener el nombre de usuario autenticado
            var nombreUsuario = User.Identity.Name;

            // Verificar si el nombre de usuario está vacío
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                return Unauthorized("Usuario no autenticado.");
            }

            // Buscar el usuario en la tabla Usuarios basado en el nombre de usuario
            var usuario = _context.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario);

            // Verificar si el usuario fue encontrado
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el staff (maestro) asociado al usuario autenticado
            var staff = _context.Staff.FirstOrDefault(s => s.IdUsuario == usuario.IdUsuario);

            // Verificar si el staff fue encontrado
            if (staff == null)
            {
                return NotFound("No se encontró el maestro relacionado con el usuario.");
            }

            // Obtener las mesas asignadas al maestro usando su IdStaff
            var mesasAsignadas = _context.AsignacionMaestros
                .Where(am => am.IdStaff == staff.IdStaff)
                .Select(am => new
                {
                    am.IdMesa,
                    NombreMesa = $"Mesa {am.IdMesa} - {am.IdMesaNavigation.NombreSedeNavigation.NombreSede} - {am.IdMesaNavigation.IdJornadaNavigation.DiaSemana} {am.IdMesaNavigation.IdJornadaNavigation.Horario}"
                })
                .ToList();

            // Pasar los datos de las mesas asignadas a la vista
            ViewBag.IdMesa = new SelectList(mesasAsignadas, "IdMesa", "NombreMesa");

            // Cargar otros datos necesarios para el formulario
            ViewBag.IdNivel = new SelectList(_context.Nivels, "IdNivel", "NombreNivel");
            ViewBag.IdBimestre = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");
            ViewBag.IdLibro = new SelectList(_context.Libros, "IdLibro", "NombreLibro");
            ViewBag.IdLeccion = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");

            return View();
        }

        // POST: AvanceMesa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNivel,IdBimestre,IdLibro,IdLeccion,IdMesa")] AvanceMesa avanceMesa)
        {
            if (ModelState.IsValid)
            {
                // Guardar el nuevo avance en la base de datos
                _context.Add(avanceMesa);
                await _context.SaveChangesAsync();

                // Mostrar un mensaje de éxito
                TempData["SuccessMessage"] = "Avance de mesa creado correctamente";
                return RedirectToAction(nameof(Index));
            }

            // Si hay algún error, recargar los datos necesarios para el formulario
            ViewBag.IdNivel = new SelectList(_context.Nivels, "IdNivel", "NombreNivel", avanceMesa.IdNivel);
            ViewBag.IdBimestre = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", avanceMesa.IdBimestre);
            ViewBag.IdLibro = new SelectList(_context.Libros, "IdLibro", "NombreLibro", avanceMesa.IdLibro);
            ViewBag.IdLeccion = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", avanceMesa.IdLeccion);
            ViewBag.IdMesa = new SelectList(_context.Mesas, "IdMesa", "IdMesa", avanceMesa.IdMesa);

            return View(avanceMesa);
        }

        // GET: AvanceMesa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avanceMesa = await _context.AvanceMesas.FindAsync(id);
            if (avanceMesa == null)
            {
                return NotFound();
            }
            // Inicializa ViewBag para la vista Edit
            ViewBag.Niveles = new SelectList(await _context.Nivels.ToListAsync(), "IdNivel", "NombreNivel", avanceMesa.IdNivel);
            ViewBag.Bimestres = new SelectList(await _context.Bimestres.ToListAsync(), "IdBimestre", "NombreBimestre", avanceMesa.IdBimestre);
            ViewBag.Libros = new SelectList(await _context.Libros.ToListAsync(), "IdLibro", "NombreLibro", avanceMesa.IdLibro);
            ViewBag.Lecciones = new SelectList(await _context.Leccions.ToListAsync(), "IdLeccion", "Descripcion", avanceMesa.IdLeccion);
            ViewBag.Mesas = new SelectList(await _context.Mesas.ToListAsync(), "IdMesa", "IdMesa", avanceMesa.IdMesa);

            return View(avanceMesa);
        }

        // POST: AvanceMesa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAvancemesa,IdNivel,IdBimestre,IdLibro,IdLeccion,IdMesa")] AvanceMesa avanceMesa)
        {
            if (id != avanceMesa.IdAvancemesa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avanceMesa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvanceMesaExists(avanceMesa.IdAvancemesa))
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

            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", avanceMesa.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", avanceMesa.IdLeccion);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "IdLibro", "NombreLibro", avanceMesa.IdLibro);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", avanceMesa.IdMesa);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "NombreNivel", avanceMesa.IdNivel);

            return View(avanceMesa);
        }

        // GET: AvanceMesa/Delete/5
        // public async Task<IActionResult> Delete(int? id)
        // {
        //     if (id == null)
        //     {
        //         return NotFound();
        //     }

        //     var avanceMesa = await _context.AvanceMesas
        //         .Include(a => a.IdBimestreNavigation)
        //         .Include(a => a.IdLeccionNavigation)
        //         .Include(a => a.IdLibroNavigation)
        //         .Include(a => a.IdMesaNavigation)
        //         .Include(a => a.IdNivelNavigation)
        //         .FirstOrDefaultAsync(m => m.IdAvancemesa == id);

        //     if (avanceMesa == null)
        //     {
        //         return NotFound();
        //     }

        //     return View(avanceMesa);
        // }

        // // POST: AvanceMesa/Delete/5
        // [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> DeleteConfirmed(int id)
        // {
        //     var avanceMesa = await _context.AvanceMesas.FindAsync(id);
        //     if (avanceMesa != null)
        //     {
        //         _context.AvanceMesas.Remove(avanceMesa);
        //         await _context.SaveChangesAsync();
        //         TempData["SuccessMessage"] = "Dato eliminado correctamente";
        //     }
        //     else
        //     {
        //         TempData["ErrorMessage"] = "Se produjo un error al eliminar el dato.";
        //     }

        //     return RedirectToAction(nameof(Index));
        // }


        private bool AvanceMesaExists(int id)
        {
            return _context.AvanceMesas.Any(e => e.IdAvancemesa == id);
        }
    }
}
