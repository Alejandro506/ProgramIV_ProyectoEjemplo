using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Libros2
{
    public int Id { get; set; }

    public string OriginalName { get; set; } = null!;

    public string SpanishName { get; set; } = null!;

    public string? Edition { get; set; }

    public int? Year { get; set; }

    public string? Editor { get; set; }
}
