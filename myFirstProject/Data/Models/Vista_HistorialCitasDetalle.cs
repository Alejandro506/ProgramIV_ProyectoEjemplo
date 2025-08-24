using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Vista_HistorialCitasDetalle
{
    public int IdHistorial { get; set; }

    public int IdCita { get; set; }

    public string? Accion { get; set; }

    public DateTime? FechaAccion { get; set; }

    public string? Observaciones { get; set; }

    public int IdAnimal { get; set; }

    public string NombreAnimal { get; set; } = null!;

    public int IdDoctor { get; set; }

    public string NombreDoctor { get; set; } = null!;

    public DateTime FechaHoraCita { get; set; }

    public string? EstadoCita { get; set; }
}
