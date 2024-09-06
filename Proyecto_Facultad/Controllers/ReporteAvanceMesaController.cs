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
    public class ReporteAvanceMesaController : Controller
    {
        private readonly BdfflContext _context;

        public ReporteAvanceMesaController(BdfflContext context)
        {
            _context = context;
        }

        // GET: AvanceMesa
        public async Task<IActionResult> ReporteAvanceMesa()
{
    var ultimoAvanceMesas = await _context.AvanceMesas
        .Include(a => a.IdNivelNavigation)
        .Include(a => a.IdBimestreNavigation)
        .Include(a => a.IdLibroNavigation)
        .Include(a => a.IdLeccionNavigation)
        .Include(a => a.IdMesaNavigation)
        .GroupBy(a => a.IdMesa)
        .Select(g => g.OrderByDescending(a => a.IdAvancemesa).FirstOrDefault())
        .ToListAsync();

    return View(ultimoAvanceMesas);
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
       
        private bool AvanceMesaExists(int id)
        {
            return _context.AvanceMesas.Any(e => e.IdAvancemesa == id);
        }
    }
}
