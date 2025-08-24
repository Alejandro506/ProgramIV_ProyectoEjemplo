using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Estudiante
{
    public int EstudianteId { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Carnet { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
