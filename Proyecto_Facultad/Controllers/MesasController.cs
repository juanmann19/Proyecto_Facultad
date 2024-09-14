using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Coordinador")]
    public class MesasController : Controller
    {
        private readonly BdfflContext _context;

        public MesasController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Mesas
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.Mesas.Include(m => m.IdJornadaNavigation).Include(m => m.NombreSedeNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: Mesas/Create
        public IActionResult Create()
        {
            ViewData["DiaSemana"] = new SelectList(_context.Jornada, "IdJornada", "DiaSemana");
            ViewData["NombreSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede");
            return View();
        }

        // POST: Mesas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMesa,IdSede,IdJornada,FechaInicio,FechaFin,EstadoMesa")] Mesa mesa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mesa);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Mesa creada con exito.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
            return View(mesa);

            // Crear una lista combinada de Día de la Semana y Horario
            var jornadaList = await _context.Jornada
                .Select(j => new
                {
                    j.IdJornada,
                    DisplayText = $"{j.DiaSemana} - {j.Horario}"
                })
                .ToListAsync();

            ViewData["Jornadas"] = new SelectList(jornadaList, "IdJornada", "DisplayText", mesa.IdJornada);
            ViewData["NombreSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede", mesa.IdSede);

            return View(mesa);
        }

        // GET: Mesas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }
            ViewData["DiaSemana"] = new SelectList(_context.Jornada, "IdJornada", "DiaSemana", mesa.IdJornada);
            ViewData["NombreSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede", mesa.IdSede);
            return View(mesa);
        }

        // POST: Mesas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMesa,IdSede,IdJornada,FechaInicio,FechaFin,EstadoMesa")] Mesa mesa)
        {
            if (id != mesa.IdMesa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mesa);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MesaExists(mesa.IdMesa))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Se produjo un error al actualizar datos.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiaSemana"] = new SelectList(_context.Jornada, "IdJornada", "DiaSemana", mesa.IdJornada);
            ViewData["NombreSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede", mesa.IdSede);
            return View(mesa);
        }

        private bool MesaExists(int id)
        {
            return _context.Mesas.Any(e => e.IdMesa == id);
        }
    }
}
