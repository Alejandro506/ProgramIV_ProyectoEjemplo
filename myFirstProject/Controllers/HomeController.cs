using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var hoyStart = hoy.ToDateTime(TimeOnly.MinValue);
        var hoyEnd   = hoy.ToDateTime(TimeOnly.MaxValue);

        ViewBag.CitasHoy        = await _db.Citas.CountAsync(c => c.FechaHora >= hoyStart && c.FechaHora <= hoyEnd);
        ViewBag.CitasAgendadas  = await _db.Citas.CountAsync(c => c.Estado == "Agendada");
        ViewBag.CitasCanceladas = await _db.Citas.CountAsync(c => c.Estado == "Cancelada");

        // Próximas 6 citas en los siguientes 7 días
        var end7 = DateTime.Today.AddDays(7).AddDays(1).AddTicks(-1);
        ViewBag.NextCitas = await _db.Citas
            .Include(c => c.IdAnimalNavigation).ThenInclude(a => a.IdClienteNavigation)
            .Include(c => c.IdDoctorNavigation)
            .Where(c => c.Estado == "Agendada" && c.FechaHora >= DateTime.Now && c.FechaHora <= end7)
            .OrderBy(c => c.FechaHora)
            .Take(6)
            .Select(c => new {
                Id = c.IdCita,
                Fecha = c.FechaHora,
                Animal = c.IdAnimalNavigation!.Nombre,
                Cliente = c.IdAnimalNavigation!.IdClienteNavigation!.Nombre,
                Doctor = c.IdDoctorNavigation!.Nombre
            })
            .ToListAsync();

        // Huecos rápidos (si ya tienes un servicio, úsalo)
        // Aquí, por simplicidad, tomamos los últimos reagendamientos libres guardados o nada.
        ViewBag.FreeSlots = new List<DateTime>(); // Puedes poblar con tu CitasService.GetDisponibilidadSemana(...)

        // Clientes con más mascotas
        ViewBag.TopClientes = await _db.Clientes
            .Select(cl => new {
                Nombre = cl.Nombre!,
                Mascotas = cl.Animales!.Count
            })
            .OrderByDescending(x => x.Mascotas)
            .Take(5)
            .ToListAsync();

        // Total animales para barra %
        ViewBag.TotalAnimales = await _db.Animales.CountAsync();

        // Animales por tipo
        ViewBag.AnimalesPorTipo = await _db.TiposAnimales
            .Select(t => new { t.Descripcion, Cant = t.Animales!.Count })
            .ToDictionaryAsync(x => x.Descripcion!, x => x.Cant);

        return View();
    }
}
