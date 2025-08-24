using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Libro
{
    public int LibroId { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Autor { get; set; }

    public string? Genero { get; set; }

    public int? AnioPublicacion { get; set; }

    public virtual ICollection<Ejemplare> Ejemplares { get; set; } = new List<Ejemplare>();
}
