namespace Proyecto_Facultad.ViewModels
{
    public class ReporteAlumnosGraduadosViewModel
    {
        public List<AlumnoReporteViewModel> AlumnosGraduados { get; set; }
    }

    public class AlumnoReporteViewModel
    {
        public string NombreCompleto { get; set; }
        public string Telefono { get; set; }
        public int NotaObtenida { get; set; }
        public string Nivel { get; set; } // Agregado para mostrar el nivel
    }
}