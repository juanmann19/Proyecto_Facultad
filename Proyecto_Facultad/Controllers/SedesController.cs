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
    public class SedesController : Controller
    {
        private readonly BdfflContext _context;

        public SedesController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Sedes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sedes.ToListAsync());
        }


        // GET: Sedes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sedes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSede,NombreSede")] Sede sede)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sede);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Dato cargado correctamente";
                return RedirectToAction(nameof(Index));
            }
            TempData["SuccessMessage"] = "Se produjo un error al guardar los datos";
            return View(sede);
        }

        // GET: Sedes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sede = await _context.Sedes.FindAsync(id);
            if (sede == null)
            {
                return NotFound();
            }
            return View(sede);
        }

        // POST: Sedes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSede,NombreSede")] Sede sede)
        {
            if (id != sede.IdSede)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sede);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Dato actualizados correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SedeExists(sede.IdSede))
                    {
                        return NotFound();
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Se produjo un error";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sede);
        }
        private bool SedeExists(int id)
        {
            return _context.Sedes.Any(e => e.IdSede == id);
        }
    }
}
