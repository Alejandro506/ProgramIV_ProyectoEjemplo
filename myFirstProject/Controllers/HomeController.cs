using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var todayStart = DateTime.Today;
        var todayEnd = todayStart.AddDays(1);

        // Citas de hoy (solo agendadas)
        var citasHoy = await _db.Citas
            .CountAsync(c => c.Estado == "Agendada" && c.FechaHora >= todayStart && c.FechaHora < todayEnd);

        // Totales
        var agendadas = await _db.Citas.CountAsync(c => c.Estado == "Agendada");
        var canceladas = await _db.Citas.CountAsync(c => c.Estado == "Cancelada");

        ViewBag.CitasHoy = citasHoy;
        ViewBag.CitasAgendadas = agendadas;
        ViewBag.CitasCanceladas = canceladas;

        return View();
    }
}
