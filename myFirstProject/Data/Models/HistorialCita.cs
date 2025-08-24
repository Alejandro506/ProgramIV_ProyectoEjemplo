using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class HistorialCita
{
    public int IdHistorial { get; set; }

    public int IdCita { get; set; }

    public string? Accion { get; set; }

    public DateTime? FechaAccion { get; set; }

    public string? Observaciones { get; set; }

    public virtual Cita IdCitaNavigation { get; set; } = null!;
}
