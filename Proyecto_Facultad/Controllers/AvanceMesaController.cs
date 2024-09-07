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
            var avances = _context.AvanceMesas
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdLibroNavigation)
                .Include(a => a.IdMesaNavigation)
                .Include(a => a.IdNivelNavigation);

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
            ViewBag.IdNivel = new SelectList(_context.Nivels, "IdNivel", "NombreNivel");
            ViewBag.IdBimestre = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");
            ViewBag.IdLibro = new SelectList(_context.Libros, "IdLibro", "NombreLibro");
            ViewBag.IdLeccion = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");
            ViewBag.IdMesa = new SelectList(_context.Mesas, "IdMesa", "IdMesa"); // Cambia "IdMesa" por el nombre que desees mostrar

            return View();
        }

        // POST: AvanceMesa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNivel,IdBimestre,IdLibro,IdLeccion,IdMesa")] AvanceMesa avanceMesa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avanceMesa);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", avanceMesa.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", avanceMesa.IdLeccion);
            ViewData["IdLibro"] = new SelectList(_context.Libros, "IdLibro", "NombreLibro", avanceMesa.IdLibro);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", avanceMesa.IdMesa);
            ViewData["IdNivel"] = new SelectList(_context.Nivels, "IdNivel", "NombreNivel", avanceMesa.IdNivel);

            return View(avanceMesa);
        }

        // GET: AvanceMesa/Edit/5
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

            // Verifica los datos que se están cargando
            var bimestres = await _context.Bimestres.ToListAsync();
            var leccions = await _context.Leccions.ToListAsync();
            var libros = await _context.Libros.ToListAsync();
            var mesas = await _context.Mesas.ToListAsync();
            var nivels = await _context.Nivels.ToListAsync();

            // Agrega datos de depuración (puedes usar un logger o simplemente inspeccionar el contenido en la vista)
            System.Diagnostics.Debug.WriteLine($"Bimestres count: {bimestres.Count}");
            System.Diagnostics.Debug.WriteLine($"Leccions count: {leccions.Count}");
            System.Diagnostics.Debug.WriteLine($"Libros count: {libros.Count}");
            System.Diagnostics.Debug.WriteLine($"Mesas count: {mesas.Count}");
            System.Diagnostics.Debug.WriteLine($"Nivels count: {nivels.Count}");

            // Inicializa ViewData para la vista Edit
            ViewData["IdBimestre"] = new SelectList(bimestres, "IdBimestre", "NombreBimestre", avanceMesa.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(leccions, "IdLeccion", "Descripcion", avanceMesa.IdLeccion);
            ViewData["IdLibro"] = new SelectList(libros, "IdLibro", "NombreLibro", avanceMesa.IdLibro);
            ViewData["IdMesa"] = new SelectList(mesas, "IdMesa", "IdMesa", avanceMesa.IdMesa);
            ViewData["IdNivel"] = new SelectList(nivels, "IdNivel", "NombreNivel", avanceMesa.IdNivel);


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

            // Re-cargar los datos para los SelectList
            ViewData["IdBimestre"] = new SelectList(await _context.Bimestres.ToListAsync(), "IdBimestre", "NombreBimestre", avanceMesa.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(await _context.Leccions.ToListAsync(), "IdLeccion", "Descripcion", avanceMesa.IdLeccion);
            ViewData["IdLibro"] = new SelectList(await _context.Libros.ToListAsync(), "IdLibro", "NombreLibro", avanceMesa.IdLibro);
            ViewData["IdMesa"] = new SelectList(await _context.Mesas.ToListAsync(), "IdMesa", "IdMesa", avanceMesa.IdMesa);
            ViewData["IdNivel"] = new SelectList(await _context.Nivels.ToListAsync(), "IdNivel", "NombreNivel", avanceMesa.IdNivel);

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
