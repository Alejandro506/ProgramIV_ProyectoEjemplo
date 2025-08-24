using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    public int EstudianteId { get; set; }

    public int EjemplarId { get; set; }

    public DateTime FechaPrestamo { get; set; }

    public DateTime? FechaDevolucion { get; set; }

    public virtual Ejemplare Ejemplar { get; set; } = null!;

    public virtual Estudiante Estudiante { get; set; } = null!;
}
