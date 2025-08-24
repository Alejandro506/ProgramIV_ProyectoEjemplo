using System;
using System.Collections.Generic;

namespace myFirstProject.Data.Models;

public partial class Animale
{
    public int IdAnimal { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdTipo { get; set; }

    public int IdCliente { get; set; }

    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual TiposAnimale IdTipoNavigation { get; set; } = null!;
}
