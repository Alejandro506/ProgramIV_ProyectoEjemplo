using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;
using myFirstProject.Services;
using myFirstProject.ViewModels;

namespace myFirstProject.Controllers;

public class CitasController : Controller
{
    private readonly AppDbContext _db;
    private readonly CitasService _svc;

    public CitasController(AppDbContext db, CitasService svc)
    {
        _db = db;
        _svc = svc;
    }

    // -------- Accesos del menú --------
    [HttpGet]
    public IActionResult Agendar() => RedirectToAction(nameof(Nueva));

    // GET /Citas/Reagendar (formulario simple)
    [HttpGet]
    public IActionResult Reagendar()
    {
        return View(new ReagendarCitaVM
        {
            // pre-carga fecha redondeada al próximo bloque de 30 min
            NuevaFechaHora = RoundToNextHalfHour(DateTime.Now)
        });
    }

    // ================= Reagendar: LISTADO (nuevo) =================
    // GET /Citas/ReagendarListado?q=ana&desde=2025-08-01&hasta=2025-08-31&incluirCanceladas=true
    [HttpGet]
    public async Task<IActionResult> ReagendarListado(string? q, DateOnly? desde, DateOnly? hasta, bool incluirCanceladas = false)
    {
        var start = (desde ?? DateOnly.FromDateTime(DateTime.Today)).ToDateTime(TimeOnly.MinValue);
        var end   = (hasta ?? DateOnly.FromDateTime(DateTime.Today.AddDays(30))).ToDateTime(TimeOnly.MaxValue);

        var query = _db.Citas
            .Include(c => c.IdAnimalNavigation)!.ThenInclude(a => a.IdClienteNavigation)
            .Include(c => c.IdDoctorNavigation)
            .Where(c => c.FechaHora >= start && c.FechaHora <= end);

        // si no se marcan canceladas, sólo mostrar "Agendada"
        if (!incluirCanceladas)
            query = query.Where(c => c.Estado == "Agendada");
        else
            query = query.Where(c => c.Estado == "Agendada" || c.Estado == "Cancelada");

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.IdAnimalNavigation!.Nombre, $"%{term}%") ||
                EF.Functions.Like(c.IdAnimalNavigation!.IdClienteNavigation!.Nombre, $"%{term}%"));
        }

        var lista = await query
            .OrderBy(c => c.FechaHora)
            .Select(c => new ReagendarItemVM
            {
                IdCita    = c.IdCita,
                FechaHora = c.FechaHora,
                Animal    = c.IdAnimalNavigation!.Nombre,
                Cliente   = c.IdAnimalNavigation!.IdClienteNavigation!.Nombre,
                Doctor    = c.IdDoctorNavigation!.Nombre,
                Estado    = c.Estado ?? ""
            })
            .ToListAsync();

        var vm = new ReagendarBusquedaVM
        {
            Q = q,
            Desde = desde,
            Hasta = hasta,
            IncluirCanceladas = incluirCanceladas,
            Citas = lista
        };

        return View("ReagendarListado", vm);
    }

    // =================  Cancelar: LISTADO + POST  =================

    // GET /Citas/Cancelar?q=ana&desde=2025-08-01&hasta=2025-08-31
    [HttpGet]
    public async Task<IActionResult> Cancelar(string? q, DateOnly? desde, DateOnly? hasta)
    {
        // Rango por defecto: hoy … hoy + 30 días
        var start = (desde ?? DateOnly.FromDateTime(DateTime.Today)).ToDateTime(TimeOnly.MinValue);
        var end   = (hasta ?? DateOnly.FromDateTime(DateTime.Today.AddDays(30))).ToDateTime(TimeOnly.MaxValue);

        var query = _db.Citas
            .Include(c => c.IdAnimalNavigation)!.ThenInclude(a => a.IdClienteNavigation)
            .Include(c => c.IdDoctorNavigation)
            .Where(c => c.Estado == "Agendada" && c.FechaHora >= start && c.FechaHora <= end);

        if (!string.IsNullOrWhiteSpace(q))
        {
            var term = q.Trim();
            query = query.Where(c =>
                EF.Functions.Like(c.IdAnimalNavigation!.Nombre, $"%{term}%") ||
                EF.Functions.Like(c.IdAnimalNavigation!.IdClienteNavigation!.Nombre, $"%{term}%"));
        }

        var lista = await query
            .OrderBy(c => c.FechaHora)
            .Select(c => new CancelarItemVM
            {
                IdCita    = c.IdCita,
                FechaHora = c.FechaHora,
                Animal    = c.IdAnimalNavigation!.Nombre,
                Cliente   = c.IdAnimalNavigation!.IdClienteNavigation!.Nombre,
                Doctor    = c.IdDoctorNavigation!.Nombre
            })
            .ToListAsync();

        var vm = new CancelarBusquedaVM
        {
            Q = q,
            Desde = desde,
            Hasta = hasta,
            Citas = lista
        };

        return View("Cancelar", vm); // tu Cancelar.cshtml
    }

    // POST /Citas/Cancelar
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(CancelarCitaVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        await _svc.CancelarCita(vm.IdCita, vm.Observaciones);
        TempData["ok"] = "Cita cancelada.";
        return RedirectToAction(nameof(Detalle), new { id = vm.IdCita });
    }

    // -------- Reporte/consulta de la semana --------
    [HttpGet]
    public async Task<IActionResult> GenerarSemana(DateOnly? week, int? idDoctor)
    {
        var weekStart = week ?? GetWeekStart(DateTime.Today); // lunes
        var start = weekStart.ToDateTime(TimeOnly.MinValue);
        var end   = start.AddDays(7);

        var doctores = await _db.Doctores
            .OrderBy(d => d.Nombre)
            .Select(d => new SelectListItem { Value = d.IdDoctor.ToString(), Text = d.Nombre })
            .ToListAsync();

        var q = _db.Citas
            .Include(c => c.IdDoctorNavigation)
            .Include(c => c.IdAnimalNavigation).ThenInclude(a => a.IdClienteNavigation)
            .Where(c => c.FechaHora >= start && c.FechaHora < end);

        if (idDoctor.HasValue) q = q.Where(c => c.IdDoctor == idDoctor.Value);

        var citas = await q.OrderBy(c => c.FechaHora)
            .Select(c => new SemanaRow
            {
                Fecha   = c.FechaHora,
                Animal  = c.IdAnimalNavigation!.Nombre,
                Cliente = c.IdAnimalNavigation!.IdClienteNavigation!.Nombre,
                Doctor  = c.IdDoctorNavigation!.Nombre,
                Estado  = c.Estado ?? ""
            })
            .ToListAsync();

        var libres = idDoctor.HasValue
            ? await _svc.GetDisponibilidadSemana(idDoctor.Value, weekStart, 30)
            : new List<DateTime>();

        var vm = new SemanaVM
        {
            WeekStart   = weekStart,
            IdDoctor    = idDoctor,
            Doctores    = doctores,
            Citas       = citas,
            Disponibles = libres
        };

        return View("GenerarSemana", vm);
    }

    // -------- Registrar --------
    [HttpGet]
    public async Task<IActionResult> Nueva()
    {
        ViewBag.Animales = await _db.Animales
            .Select(a => new { a.IdAnimal, a.Nombre })
            .OrderBy(a => a.Nombre)
            .ToListAsync();

        ViewBag.Doctores = await _db.Doctores
            .Select(d => new { d.IdDoctor, d.Nombre })
            .OrderBy(d => d.Nombre)
            .ToListAsync();

        var rounded = RoundToNextHalfHour(DateTime.Now);
        return View(new RegistrarCitaVM { FechaHora = rounded });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nueva(RegistrarCitaVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        try
        {
            var id = await _svc.RegistrarCita(vm.IdAnimal, vm.IdDoctor, vm.FechaHora, vm.Observaciones);
            TempData["ok"] = $"Cita registrada. Id = {id}";
            return RedirectToAction(nameof(Detalle), new { id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(vm);
        }
    }

    // -------- Ver / Historial --------
    public async Task<IActionResult> Detalle(int id)
    {
        var cita = await _db.Citas
            .Include(c => c.IdAnimalNavigation)
            .Include(c => c.IdDoctorNavigation)
            .FirstOrDefaultAsync(c => c.IdCita == id);

        if (cita == null) return NotFound();

        ViewBag.Historial = await _svc.VerHistorialCita(id);
        return View(cita);
    }

    // -------- Reagendar (POST) --------
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reagendar(ReagendarCitaVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        await _svc.ReagendarCita(vm.IdCita, vm.NuevaFechaHora, vm.Observaciones);
        TempData["ok"] = "Cita reagendada.";
        return RedirectToAction(nameof(Detalle), new { id = vm.IdCita });
    }

    // -------- APIs de apoyo --------
    [HttpGet]
    public async Task<IActionResult> BuscarTerm(string q)
    {
        q = (q ?? "").Trim();
        if (q.Length == 0) return Json(Array.Empty<object>());

        var animales = await _db.Animales
            .Include(a => a.IdClienteNavigation)
            .Where(a =>
                EF.Functions.Like(a.Nombre, $"%{q}%") ||
                EF.Functions.Like(a.IdClienteNavigation!.Nombre, $"%{q}%"))
            .OrderBy(a => a.Nombre)
            .Take(20)
            .Select(a => new
            {
                a.IdAnimal,
                Animal  = a.Nombre,
                Cliente = a.IdClienteNavigation!.Nombre,
                a.IdCliente
            })
            .ToListAsync();

        return Json(animales);
    }

    [HttpGet]
    public async Task<IActionResult> Disponibilidad(int idDoctor, DateOnly week)
    {
        var slots = await _svc.GetDisponibilidadSemana(idDoctor, week);
        return Json(slots);
    }

    // Helpers
    private static DateOnly GetWeekStart(DateTime date)
    {
        var diff = ((int)date.DayOfWeek + 6) % 7; // lunes
        return DateOnly.FromDateTime(date.Date.AddDays(-diff));
    }

    private static DateTime RoundToNextHalfHour(DateTime dt)
    {
        var minute = dt.Minute < 30 ? 30 : 60;
        var baseHour = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
        return baseHour.AddMinutes(minute);
    }
}
