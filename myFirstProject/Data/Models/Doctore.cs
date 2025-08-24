using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Doctore
{
    public int IdDoctor { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Especialidad { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual ICollection<HorariosDoctor> HorariosDoctors { get; set; } = new List<HorariosDoctor>();
}
