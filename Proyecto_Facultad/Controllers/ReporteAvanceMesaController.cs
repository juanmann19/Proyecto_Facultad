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

        // GET: ReporteAvanceMesa/FiltrarPorNivel
        public async Task<IActionResult> FiltrarPorNivel(int? nivelId)
        {
            // Obtener la lista de niveles para el filtro
            var niveles = await _context.Nivels.ToListAsync();
            ViewBag.Niveles = niveles.Select(n => new SelectListItem
            {
                Value = n.IdNivel.ToString(),
                Text = n.NombreNivel
            }).ToList();

            IQueryable<AvanceMesa> query = _context.AvanceMesas
                .Include(a => a.IdBimestreNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdLibroNavigation)
                .Include(a => a.IdMesaNavigation);

            if (nivelId.HasValue && nivelId.Value > 0)
            {
                query = query.Where(a => a.IdNivel == nivelId.Value);
            }

            // Obtener el último dato ingresado por IdMesa después de aplicar el filtro
            var ultimoAvancePorMesa = await query
                .GroupBy(a => a.IdMesa)
                .Select(g => g.OrderByDescending(a => a.IdAvancemesa).FirstOrDefault())
                .ToListAsync();

            return View(ultimoAvancePorMesa);
        }

        // GET: ReporteAvanceMesa/FiltrarPorBimestre
        public async Task<IActionResult> FiltrarPorBimestre(int? bimestreId)
        {
            // Obtener la lista de bimestres para el filtro
            var bimestres = await _context.Bimestres.ToListAsync();
            ViewBag.Bimestres = bimestres.Select(b => new SelectListItem
            {
                Value = b.IdBimestre.ToString(),
                Text = b.NombreBimestre
            }).ToList();

            IQueryable<AvanceMesa> query = _context.AvanceMesas
                .Include(a => a.IdNivelNavigation)
                .Include(a => a.IdLeccionNavigation)
                .Include(a => a.IdLibroNavigation)
                .Include(a => a.IdMesaNavigation);

            if (bimestreId.HasValue && bimestreId.Value > 0)
            {
                query = query.Where(a => a.IdBimestre == bimestreId.Value);
            }

            // Obtener el último dato ingresado por IdMesa después de aplicar el filtro
            var ultimoAvancePorMesa = await query
                .GroupBy(a => a.IdMesa)
                .Select(g => g.OrderByDescending(a => a.IdAvancemesa).FirstOrDefault())
                .ToListAsync();

            return View(ultimoAvancePorMesa);
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
