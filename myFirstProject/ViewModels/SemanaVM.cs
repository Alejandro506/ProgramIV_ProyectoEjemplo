using Microsoft.AspNetCore.Mvc.Rendering;

namespace myFirstProject.ViewModels
{
    // VM para la pantalla "Generar Semana" (filtros/doctor + slots/detalle)
    public class SemanaVM
    {
        public DateOnly WeekStart { get; set; }
        public int? IdDoctor { get; set; }
        public List<SelectListItem> Doctores { get; set; } = new();

        // Detalle de citas de la semana (si decides mostrarlo)
        public List<SemanaRow> Citas { get; set; } = new();

        // Slots disponibles calculados por el servicio
        public List<DateTime> Disponibles { get; set; } = new();
    }

    // Fila de detalle de una cita (opcional para vistas de detalle)
    public class SemanaRow
    {
        public DateTime Fecha { get; set; }
        public string Animal  { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Doctor  { get; set; } = "";
        public string Estado  { get; set; } = "";
    }

    // Fila del reporte resumido por día (para ReporteSemana.cshtml)
    public class SemanaResumenRow
    {
        public DateOnly Fecha { get; set; }
        public int Total { get; set; }
        public int Agendadas { get; set; }
        public int Canceladas { get; set; }
    }
}
