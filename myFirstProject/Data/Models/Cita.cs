using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Cita
{
    public int IdCita { get; set; }

    public int IdAnimal { get; set; }

    public int IdDoctor { get; set; }

    public DateTime FechaHora { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<HistorialCita> HistorialCita { get; set; } = new List<HistorialCita>();

    public virtual Animale IdAnimalNavigation { get; set; } = null!;

    public virtual Doctore IdDoctorNavigation { get; set; } = null!;
}
