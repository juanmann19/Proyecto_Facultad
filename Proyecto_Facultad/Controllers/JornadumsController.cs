using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Coordinador, Admin")]
    public class JornadumsController : Controller
    {
        private readonly BdfflContext _context;

        public JornadumsController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Jornadums
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.Jornada.Include(j => j.IdSedeNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: Jornadums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornadum = await _context.Jornada
                .Include(j => j.IdSedeNavigation)
                .FirstOrDefaultAsync(m => m.IdJornada == id);
            if (jornadum == null)
            {
                return NotFound();
            }

            return View(jornadum);
        }

        // GET: Jornadums/Create
        public IActionResult Create()
        {
            ViewData["IdSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede");
            return View();
        }

        // POST: Jornadums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJornada,DiaSemana,Horario,IdSede")] Jornadum jornadum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jornadum);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Jornada creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede", jornadum.IdSede);
            TempData["ErrorMessage"] = "Se produjo un error al guardar los datos.";
            return View(jornadum);
        }

        // GET: Jornadums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jornadum = await _context.Jornada.FindAsync(id);
            if (jornadum == null)
            {
                return NotFound();
            }
            ViewData["IdSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede", jornadum.IdSede);
            return View(jornadum);
        }

        // POST: Jornadums/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJornada,DiaSemana,Horario,IdSede")] Jornadum jornadum)
        {
            if (id != jornadum.IdJornada)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jornadum);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Datos actualizados correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JornadumExists(jornadum.IdJornada))
                    {
                        return NotFound();
                    }

                    else
                    {
                        TempData["ErrorMessage"] = "Se produjo un error al actualizar la jornada.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Error al actualizar la jornada.";
            ViewData["IdSede"] = new SelectList(_context.Sedes, "IdSede", "NombreSede", jornadum.IdSede);
            return View(jornadum);
        } 

            private bool JornadumExists(int id)
        {
            return _context.Jornada.Any(e => e.IdJornada == id);
        }
    }
}
