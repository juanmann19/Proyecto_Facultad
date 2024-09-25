using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Proyecto_Facultad.Models;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Coordinador, Admin")]
    public class StaffsController : Controller
    {
        private readonly BdfflContext _context;

        public StaffsController(BdfflContext context)
        {
            _context = context;
        }

        // GET: Staffs
        public async Task<IActionResult> Index()
        {
            var bdfflContext = _context.Staff.Include(s => s.IdUsuarioNavigation);
            return View(await bdfflContext.ToListAsync());
        }

        // GET: Staffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff
                .Include(s => s.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdStaff == id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staffs/Create
        public IActionResult Create()
        {
            ViewBag.Usuarios = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario");
            return View();
        }

        // POST: Staffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStaff,PrimerNombreStaff,SegundoNombreStaff,OtrosNombresStaff,PrimerApellidoStaff,SegundoApellidoStaff,ApellidoCasado,FechaNacimientoStaff,FechaBautizoStaff,Direccion,Telefono,Dpi,Nit,EstadoCivil,Genero,NumeroCelula,NivelAprobado,CodigoFrater,EstatusStaff,IdUsuario")] Staff staff)
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "ContrasenaUsuario", staff.IdUsuario);
            if (ModelState.IsValid)
            {
                // Validar que el usuario existe
                var usuario = await _context.Usuarios.FindAsync(staff.IdUsuario);
                if (usuario == null)
                {
                    ModelState.AddModelError("IdUsuario", "El usuario especificado no existe.");

                }
                else
                {
                    // Validar que el usuario no esté asignado a otro Staff
                    var existingStaff = await _context.Staff.AnyAsync(s => s.IdUsuario == staff.IdUsuario);
                    if (existingStaff)
                    {
                        ModelState.AddModelError("IdUsuario", "El usuario ya existe. NO es posible asignarlo");
                    }
                    else
                    {
                        staff.EstatusStaff = true;
                        _context.Add(staff);
                        await _context.SaveChangesAsync();
                        TempData["SuccessMessage"] = "Creado correctamente";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(staff);
        }

        // GET: Staffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            ViewBag.Usuarios = new SelectList(_context.Usuarios, "IdUsuario", "NombreUsuario");
            return View(staff);
        }

        // POST: Staffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStaff,PrimerNombreStaff,SegundoNombreStaff,OtrosNombresStaff,PrimerApellidoStaff,SegundoApellidoStaff,ApellidoCasado,FechaNacimientoStaff,FechaBautizoStaff,Direccion,Telefono,Dpi,Nit,EstadoCivil,Genero,NumeroCelula,NivelAprobado,CodigoFrater,EstatusStaff,IdUsuario")] Staff staff)
        {
            if (id != staff.IdStaff)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(staff);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Actualizado correctamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaffExists(staff.IdStaff))
                    {
                        return NotFound();
                    }
                    else
                    {

                        TempData["ErrorMessage"] = "Se produjo un error al actualizar.";
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", staff.IdUsuario);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var staff = await _context.Staff
        //        .Include(s => s.IdUsuarioNavigation)
        //        .FirstOrDefaultAsync(m => m.IdStaff == id);
        //    if (staff == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(staff);
        //}

        // POST: Staffs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var staff = await _context.Staff.FindAsync(id);
        //    if (staff != null)
        //    {
        //        _context.Staff.Remove(staff);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        public async Task<IActionResult> Deactivate(int? id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            // Cambiar el estatus
            staff.EstatusStaff = !staff.EstatusStaff;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = staff.EstatusStaff ? "Staff activado correctamente" : "Staff desactivado correctamente";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffExists(staff.IdStaff))
                {
                    return NotFound();
                }
                else
                {
                    TempData["ErrorMessage"] = "Se produjo un error al cambiar el estatus.";
                    throw;
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StaffExists(int id)
        {
            return _context.Staff.Any(e => e.IdStaff == id);
        }
    }
}
