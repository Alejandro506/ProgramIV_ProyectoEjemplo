namespace myFirstProject.ViewModels;

public class RegistrarCitaVM
{
    public int IdAnimal { get; set; }
    public int IdDoctor { get; set; }
    public DateTime FechaHora { get; set; }
    public string Observaciones { get; set; } = "";
}

public class ReagendarCitaVM
{
    public int IdCita { get; set; }
    public DateTime NuevaFechaHora { get; set; }
    public string Observaciones { get; set; } = "";
}

public class CancelarCitaVM
{
    public int IdCita { get; set; }
    public string Observaciones { get; set; } = "";
}
