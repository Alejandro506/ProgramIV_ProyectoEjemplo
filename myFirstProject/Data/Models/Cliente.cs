using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Telefono { get; set; }

    public virtual ICollection<Animale> Animales { get; set; } = new List<Animale>();
}
