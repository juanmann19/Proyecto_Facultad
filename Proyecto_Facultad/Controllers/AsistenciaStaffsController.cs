﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;

namespace Proyecto_Facultad.Controllers
{
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
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre");
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion");
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa");
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado");
            return View();
        }

        // POST: AsistenciaStaffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAsistenciaStaff,IdStaff,FechaClase,IdMesa,IdLeccion,IdBimestre,Ausencia")] AsistenciaStaff asistenciaStaff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(asistenciaStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asistenciaStaff.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", asistenciaStaff.IdStaff);
            return View(asistenciaStaff);
        }

        // GET: AsistenciaStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var asistenciaStaff = await _context.AsistenciaStaffs.FindAsync(id);
            if (asistenciaStaff == null)
            {
                return NotFound();
            }
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asistenciaStaff.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", asistenciaStaff.IdStaff);
            return View(asistenciaStaff);
        }

        // POST: AsistenciaStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    _context.Update(asistenciaStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AsistenciaStaffExists(asistenciaStaff.IdAsistenciaStaff))
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
            ViewData["IdBimestre"] = new SelectList(_context.Bimestres, "IdBimestre", "NombreBimestre", asistenciaStaff.IdBimestre);
            ViewData["IdLeccion"] = new SelectList(_context.Leccions, "IdLeccion", "Descripcion", asistenciaStaff.IdLeccion);
            ViewData["IdMesa"] = new SelectList(_context.Mesas, "IdMesa", "IdMesa", asistenciaStaff.IdMesa);
            ViewData["IdStaff"] = new SelectList(_context.Staff, "IdStaff", "NivelAprobado", asistenciaStaff.IdStaff);
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