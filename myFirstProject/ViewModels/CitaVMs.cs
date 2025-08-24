namespace myFirstProject.ViewModels
{
    // Para listados generales (si lo necesitas en otras pantallas)
    public class CitaSimpleVM
    {
        public int IdCita { get; set; }
        public string Animal { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Doctor { get; set; } = "";
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = "";
    }

    // ===== Cancelar: filtros + resultado (usa CancelarItemVM) =====
    public class CancelarBusquedaVM
    {
        public string? Q { get; set; }              // término (animal o cliente)
        public int? IdDoctor { get; set; }          // opcional (si luego lo agregas)
        public DateOnly? Desde { get; set; }        // rango opcional
        public DateOnly? Hasta { get; set; }

        public List<CancelarItemVM> Citas { get; set; } = new(); // <-- CAMBIO

    }

    // Fila mostrada en el listado de cancelación
    public class CancelarItemVM
    {
        public int IdCita { get; set; }
        public DateTime FechaHora { get; set; }
        public string Animal { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Doctor { get; set; } = "";
    }

    // POST cancelar
    public class CancelarCitaVM
    {
        public int IdCita { get; set; }
        public string? Observaciones { get; set; }
    }

    // POST reagendar
    public class ReagendarCitaVM
    {
        public int IdCita { get; set; }
        public DateTime NuevaFechaHora { get; set; }
        public string? Observaciones { get; set; }
    }

    // POST registrar
    public class RegistrarCitaVM
    {
        public int IdAnimal { get; set; }
        public int IdDoctor { get; set; }
        public DateTime FechaHora { get; set; }
        public string? Observaciones { get; set; }
    }

    // Filtros + resultado para el listado de reagendar
    public class ReagendarBusquedaVM
    {
        public string? Q { get; set; }                // término (animal o cliente)
        public int? IdDoctor { get; set; }            // filtro opcional
        public DateOnly? Desde { get; set; }
        public DateOnly? Hasta { get; set; }

        // marcar para ver también las canceladas
        public bool IncluirCanceladas { get; set; } = false;

        public List<ReagendarItemVM> Citas { get; set; } = new();
    }

    // Fila del listado de reagendar
    public class ReagendarItemVM
    {
        public int IdCita { get; set; }
        public DateTime FechaHora { get; set; }
        public string Animal { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Doctor  { get; set; } = "";
        public string Estado  { get; set; } = "";
    }

}
