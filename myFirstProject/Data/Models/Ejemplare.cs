using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Ejemplare
{
    public int EjemplarId { get; set; }

    public int LibroId { get; set; }

    public string CodigoInventario { get; set; } = null!;

    public string? Estado { get; set; }

    public virtual Libro Libro { get; set; } = null!;

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
