namespace myFirstProject.ViewModels
{
    public class CitasListadoFiltroVM
    {
        public string? Q { get; set; }
        public int? IdDoctor { get; set; }
        public string? Estado { get; set; }
        public DateOnly? Desde { get; set; }
        public DateOnly? Hasta { get; set; }
    }

    public class CitasListadoItemVM
    {
        public int IdCita { get; set; }
        public DateTime FechaHora { get; set; }
        public string Animal { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Doctor { get; set; } = "";
        public string Estado { get; set; } = "";
    }

    public class CitasListadoVM
    {
        public CitasListadoFiltroVM Filtro { get; set; } = new();
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Doctores { get; set; } = new();
        public List<CitasListadoItemVM> Citas { get; set; } = new();
    }
}
