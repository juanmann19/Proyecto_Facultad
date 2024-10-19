using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Proyecto_Facultad.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Proyecto_Facultad.Controllers 
{
public class PrivacyController : Controller
{
    public IActionResult GetManual()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Manual.pdf");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", "Manual.pdf");
        }

        public IActionResult PreviewManual()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Manual.pdf");
            if (!System.IO.File.Exists(filePath))
            {
                return Content("El archivo no se encontró. Asegúrate de que 'Manual.pdf' esté en la carpeta 'wwwroot'.");
            }
            return File(System.IO.File.OpenRead(filePath), "application/pdf");
        }
    }

}