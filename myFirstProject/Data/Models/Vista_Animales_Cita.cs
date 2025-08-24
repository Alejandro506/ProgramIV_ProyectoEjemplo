using System;
using Microsoft.EntityFrameworkCore;

namespace myFirstProject.Data.Models;

// Esta clase representa la vista SQL "Vista_Animales_Cita"
[Keyless]   // Indica que la vista no tiene clave primaria
public partial class Vista_Animales_Cita
{
    public string NombreAnimal { get; set; } = null!;

    public string NombreCliente { get; set; } = null!;

    public string? NombreDoctor { get; set; }

    public DateTime? FechaHora { get; set; }

    public string? Estado { get; set; }
}
