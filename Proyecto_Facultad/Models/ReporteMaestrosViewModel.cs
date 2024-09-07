namespace Proyecto_Facultad.Models
{
    public class ReporteMaestrosViewModel
    {
        public int MaestrosAsignados { get; set; }
        public int MaestrosDisponibles { get; set; }
        public List<MaestroInfo> MaestrosNoAsignados { get; set; } = new List<MaestroInfo>();
        public string SearchTerm { get; set; }
    }

    public class MaestroInfo
    {
        public int IdStaff { get; set; }
        public string NombreCompleto { get; set; }
        public bool EstaAsignado { get; set; }
    }
}

