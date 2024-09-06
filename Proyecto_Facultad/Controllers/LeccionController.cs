using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;

namespace Proyecto_Facultad.Controllers
{
    public class LeccionController : Controller
    {
        private readonly BdfflContext _context;

        public LeccionController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Leccion
        public async Task<IActionResult> Index()
        {
            var leccionesConLibros = await _context.Leccions
        .Include(l => l.IdLibroNavigation) // Incluye la información del libro
        .ToListAsync();

            return View(leccionesConLibros);
        }

        // GET: Leccion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Leccions
               .FirstOrDefaultAsync(m => m.IdLeccion == id);
            if (leccion == null)
            {
                return NotFound();
            }

            return View(leccion);
        }

        // GET: Leccion/Create
        public IActionResult Create()
        {
            // Obtener la lista de libros para el desplegable
            ViewBag.IdLibro = new SelectList(_context.Libros, "IdLibro", "NombreLibro");
            return View();
        }

        // POST: Leccion/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descripcion,IdLibro")] Leccion leccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leccion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            // Reinitialize the ViewBag if the model state is not valid
            ViewBag.IdLibro = new SelectList(_context.Libros, "IdLibro", "NombreLibro", leccion.IdLibro);
            TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
            return View(leccion);
        }

        // GET: Leccion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Leccions.FindAsync(id);
            if (leccion == null)
            {
                return NotFound();
            }

            // Obtener la lista de libros para el desplegable
            ViewBag.IdLibro = new SelectList(_context.Libros, "IdLibro", "NombreLibro", leccion.IdLibro);
            return View(leccion);
        }

        // POST: Leccion/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLeccion,Descripcion,IdLibro")] Leccion leccion)
        {
            if (id != leccion.IdLeccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leccion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeccionExists(leccion.IdLeccion))
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

            // Reinitialize the ViewBag if the model state is not valid
            ViewBag.IdLibro = new SelectList(_context.Libros, "IdLibro", "NombreLibro", leccion.IdLibro);
            return View(leccion);
        }

        private bool LeccionExists(int id)
        {
            return _context.Leccions.Any(e => e.IdLeccion == id);
        }        // GET: Leccion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Leccions
                .FirstOrDefaultAsync(m => m.IdLeccion == id);
            if (leccion == null)
            {
                return NotFound();
            }

            return View(leccion);
        }

        // POST: Leccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var leccion = await _context.Leccions.FindAsync(id);
                if (leccion != null)
                {
                    _context.Leccions.Remove(leccion);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Lecci�n eliminada correctamente";
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Se produjo un error al eliminar la lecci�n.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

