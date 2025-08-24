using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class HorariosDoctor
{
    public int IdHorario { get; set; }

    public int IdDoctor { get; set; }

    public string DiaSemana { get; set; } = null!;

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual Doctore IdDoctorNavigation { get; set; } = null!;
}
