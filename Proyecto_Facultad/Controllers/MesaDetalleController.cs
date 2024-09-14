using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Facultad.Controllers
{
    [Authorize (Roles = "Coordinador, Auxiliar, Maestro")]
    public class MesaDetalleController : Controller
    {
        private readonly BdfflContext _context;

        public MesaDetalleController(BdfflContext context)
        {
            _context = context;
        }

        // Acción para listar las mesas
        public async Task<IActionResult> Index()
        {
            var mesas = await _context.Mesas
                .Include(m => m.NombreSedeNavigation)
                .Include(m => m.IdJornadaNavigation)
                .Select(m => new
            {
                m.IdMesa,
                SedeDescripcion = m.NombreSedeNavigation.NombreSede,
                JornadaDescripcion = $"{m.IdJornadaNavigation.DiaSemana} {m.IdJornadaNavigation.Horario}",
                m.FechaInicio,
                FechaFin = m.FechaFin.HasValue ? m.FechaFin.Value.ToString("dd/MM/yyyy") : "N/A",
                EstadoMesa = m.EstadoMesa ? "Activa" : "Inactiva"
            })
            .ToListAsync();

            return View(mesas);
        }
        
        public async Task<IActionResult> Detalle(int id)
        {
            var mesa = await _context.Mesas
                .Include(m => m.AsignacionAlumnos)
                    .ThenInclude(a => a.IdAlumnoNavigation) 
                .Include(m => m.AsignacionMaestros)
                    .ThenInclude(a => a.IdStaffNavigation) 
                .FirstOrDefaultAsync(m => m.IdMesa == id);

            if (mesa == null)
            {
                return NotFound();
            }

            var vistaModelo = new MesaDetalleViewModel
            {
                Mesa = mesa,
                Alumnos = mesa.AsignacionAlumnos.Select(a => new AlumnoViewModel
                {
                    IdAlumno = a.IdAlumno,
                    NombreCompleto = $"{a.IdAlumnoNavigation.PrimerNombreAlumno} {a.IdAlumnoNavigation.SegundoNombreAlumno} {a.IdAlumnoNavigation.PrimerApellidoAlumno} {a.IdAlumnoNavigation.SegundoApellidoAlumno}"
                }).ToList(),
                Maestros = mesa.AsignacionMaestros.Select(a => new MaestroViewModel
                {
                    IdStaff = a.IdStaff,
                    NombreCompleto = $"{a.IdStaffNavigation.PrimerNombreStaff} {a.IdStaffNavigation.SegundoNombreStaff} {a.IdStaffNavigation.PrimerApellidoStaff} {a.IdStaffNavigation.SegundoApellidoStaff}"
                }).ToList()
            };

            return View(vistaModelo);
        }
    }

    public class MesaDetalleViewModel
    {
        public Mesa Mesa { get; set; }
        public List<AlumnoViewModel> Alumnos { get; set; }
        public List<MaestroViewModel> Maestros { get; set; }
    }

    public class AlumnoViewModel
    {
        public int IdAlumno { get; set; }
        public string NombreCompleto { get; set; }
    }

    public class MaestroViewModel
    {
        public int IdStaff { get; set; }
        public string NombreCompleto { get; set; }
    }
}
