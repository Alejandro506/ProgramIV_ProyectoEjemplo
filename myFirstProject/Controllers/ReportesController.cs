using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Controllers;

public class ReportesController : Controller
{
    private readonly AppDbContext _db;

    public ReportesController(AppDbContext db) => _db = db;

    // =============== 1) Animales con citas (existente, compatible) ===============
    // GET: /Reportes/AnimalesConCitas
    [HttpGet]
    public async Task<IActionResult> AnimalesConCitas()
    {
        var rows = await _db.Citas
            .Include(c => c.IdAnimalNavigation)!.ThenInclude(a => a.IdClienteNavigation)
            .Include(c => c.IdDoctorNavigation)
            .OrderByDescending(c => c.FechaHora)
            .Take(1000)
            .Select(c => new RowAnimalCita
            {
                IdCita = c.IdCita,
                FechaHora = c.FechaHora,
                Animal = c.IdAnimalNavigation!.Nombre!,
                Cliente = c.IdAnimalNavigation!.IdClienteNavigation!.Nombre!,
                Doctor = c.IdDoctorNavigation!.Nombre!,
                Estado = c.Estado ?? "Agendada"
            })
            .ToListAsync();

        return View(rows);
    }

    // =============== 2) Animales con X días sin asistir ===============
    // GET: /Reportes/AnimalesSinAsistir?dias=180
    [HttpGet]
    public async Task<IActionResult> AnimalesSinAsistir(int dias = 180)
    {
        var cutoff = DateTime.Today.AddDays(-dias);

        var rows = await _db.Animales
            .Include(a => a.IdClienteNavigation)
            .Select(a => new RowAnimalInactivo
            {
                IdAnimal = a.IdAnimal,
                Animal = a.Nombre!,
                Cliente = a.IdClienteNavigation!.Nombre!,
                UltimaCita = _db.Citas
                    .Where(c => c.IdAnimal == a.IdAnimal)
                    .Select(c => (DateTime?)c.FechaHora)
                    .Max()
            })
            .Where(r => r.UltimaCita == null || r.UltimaCita < cutoff)
            .OrderBy(r => r.Cliente).ThenBy(r => r.Animal)
            .ToListAsync();

        ViewBag.Dias = dias;
        return View(rows);
    }

    // =============== 3) Lista de animales asignados a un cliente ===============
    // GET: /Reportes/AnimalesDeCliente?idCliente=1
    [HttpGet]
    public async Task<IActionResult> AnimalesDeCliente(int? idCliente)
    {
        // para el selector
        ViewBag.Clientes = await _db.Clientes
            .OrderBy(c => c.Nombre)
            .Select(c => new SelectListItem { Value = c.IdCliente.ToString(), Text = c.Nombre! })
            .ToListAsync();

        if (!idCliente.HasValue)
        {
            ViewBag.Rows = new List<RowAnimalCliente>();
            return View();
        }

        var rows = await _db.Animales
            .Include(a => a.IdClienteNavigation)
            .Where(a => a.IdCliente == idCliente.Value)
            .Select(a => new RowAnimalCliente
            {
                IdAnimal = a.IdAnimal,
                Animal = a.Nombre!,
                Cliente = a.IdClienteNavigation!.Nombre!,
                TotalCitas = _db.Citas.Count(c => c.IdAnimal == a.IdAnimal),
                UltimaCita = _db.Citas
                    .Where(c => c.IdAnimal == a.IdAnimal)
                    .Select(c => (DateTime?)c.FechaHora)
                    .Max()
            })
            .OrderBy(a => a.Animal)
            .ToListAsync();

        ViewBag.Rows = rows;
        ViewBag.IdCliente = idCliente;
        return View();
    }

    // =============== 4) Resumen de citas por doctor ===============
    // GET: /Reportes/CitasPorDoctor?desde=2025-08-01&hasta=2025-08-31&idDoctor=1
    [HttpGet]
    public async Task<IActionResult> CitasPorDoctor(DateOnly? desde, DateOnly? hasta, int? idDoctor)
    {
        var start = (desde ?? DateOnly.FromDateTime(DateTime.Today.AddDays(-30))).ToDateTime(TimeOnly.MinValue);
        var end   = (hasta ?? DateOnly.FromDateTime(DateTime.Today)).ToDateTime(TimeOnly.MaxValue);

        ViewBag.Doctores = await _db.Doctores
            .OrderBy(d => d.Nombre)
            .Select(d => new SelectListItem { Value = d.IdDoctor.ToString(), Text = d.Nombre! })
            .ToListAsync();

        var q = _db.Citas
            .Include(c => c.IdDoctorNavigation)
            .Where(c => c.FechaHora >= start && c.FechaHora <= end);

        if (idDoctor.HasValue) q = q.Where(c => c.IdDoctor == idDoctor.Value);

        var rows = await q
            .GroupBy(c => c.IdDoctorNavigation!.Nombre!)
            .Select(g => new RowCitasDoctor
            {
                Doctor = g.Key,
                Total = g.Count(),
                Agendadas = g.Count(x => x.Estado == "Agendada" || x.Estado == null),
                Canceladas = g.Count(x => x.Estado == "Cancelada"),
                UltimaCita = g.Max(x => x.FechaHora)
            })
            .OrderBy(r => r.Doctor)
            .ToListAsync();

        ViewBag.Desde = desde;
        ViewBag.Hasta = hasta;
        ViewBag.IdDoctor = idDoctor;

        return View(rows);
    }

    // =============== VMs locales ===============
    public class RowAnimalCita
    {
        public int IdCita { get; set; }
        public DateTime FechaHora { get; set; }
        public string Animal { get; set; } = "";
        public string Cliente { get; set; } = "";
        public string Doctor { get; set; } = "";
        public string Estado { get; set; } = "Agendada";
    }

    public class RowAnimalInactivo
    {
        public int IdAnimal { get; set; }
        public string Animal { get; set; } = "";
        public string Cliente { get; set; } = "";
        public DateTime? UltimaCita { get; set; }
    }

    public class RowAnimalCliente
    {
        public int IdAnimal { get; set; }
        public string Animal { get; set; } = "";
        public string Cliente { get; set; } = "";
        public int TotalCitas { get; set; }
        public DateTime? UltimaCita { get; set; }
    }

    public class RowCitasDoctor
    {
        public string Doctor { get; set; } = "";
        public int Total { get; set; }
        public int Agendadas { get; set; }
        public int Canceladas { get; set; }
        public DateTime UltimaCita { get; set; }
    }
}
