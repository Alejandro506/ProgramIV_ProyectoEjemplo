using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class TiposAnimale
{
    public int IdTipo { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Animale> Animales { get; set; } = new List<Animale>();
}
