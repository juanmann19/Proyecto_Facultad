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
    private const string ManualFileName = "Manual.pdf";
    private const string GuiaFileName = "Manual-Tecnico.pdf"; // Nuevo archivo
    private readonly string _manualFilePath;
    private readonly string _guiaFilePath; // Ruta para el nuevo archivo

    public PrivacyController()
    {
        _manualFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ManualFileName);
        _guiaFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", GuiaFileName); // Inicializa la ruta del nuevo archivo
    }

    public IActionResult GetManual()
    {
        if (!System.IO.File.Exists(_manualFilePath))
        {
            return NotFound("El archivo Manual.pdf no se encontró.");
        }
        return File(System.IO.File.ReadAllBytes(_manualFilePath), "application/pdf", ManualFileName);
    }

    public IActionResult PreviewManual()
    {
        if (!System.IO.File.Exists(_manualFilePath))
        {
            return Content("El archivo no se encontró. Asegúrate de que 'Manual.pdf' esté en la carpeta 'wwwroot'.");
        }
        return File(System.IO.File.OpenRead(_manualFilePath), "application/pdf");
    }

    // Métodos para el nuevo archivo
    public IActionResult GetGuia()
    {
        if (!System.IO.File.Exists(_guiaFilePath))
        {
            return NotFound("El archivo Guia.pdf no se encontró.");
        }
        return File(System.IO.File.ReadAllBytes(_guiaFilePath), "application/pdf", GuiaFileName);
    }

    public IActionResult PreviewGuia()
    {
        if (!System.IO.File.Exists(_guiaFilePath))
        {
            return Content("El archivo no se encontró. Asegúrate de que 'Guia.pdf' esté en la carpeta 'wwwroot'.");
        }
        return File(System.IO.File.OpenRead(_guiaFilePath), "application/pdf");
    }
}
}