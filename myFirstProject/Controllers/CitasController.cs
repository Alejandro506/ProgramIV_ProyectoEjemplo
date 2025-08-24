using Microsoft.AspNetCore.Mvc;
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

    // -------- Registrar --------
    [HttpGet]
    public async Task<IActionResult> Nueva()
    {
        ViewBag.Animales = await _db.Animales
            .Select(a => new { a.IdAnimal, a.Nombre }).ToListAsync();
        ViewBag.Doctores = await _db.Doctores
            .Select(d => new { d.IdDoctor, d.Nombre }).ToListAsync();
        return View(new RegistrarCitaVM { FechaHora = DateTime.Now.AddHours(1) });
    }

    [HttpPost]
    public async Task<IActionResult> Nueva(RegistrarCitaVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        try
        {
            var id = await _svc.RegistrarCita(vm.IdAnimal, vm.IdDoctor, vm.FechaHora, vm.Observaciones);
            TempData["ok"] = $"Cita registrada. Id = {id}";
            return RedirectToAction(nameof(Detalle), new { id = id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
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

        var historial = await _svc.VerHistorialCita(id);
        ViewBag.Historial = historial;
        return View(cita);
    }

    // -------- Reagendar --------
    [HttpPost]
    public async Task<IActionResult> Reagendar(ReagendarCitaVM vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _svc.ReagendarCita(vm.IdCita, vm.NuevaFechaHora, vm.Observaciones);
        TempData["ok"] = "Cita reagendada.";
        return RedirectToAction(nameof(Detalle), new { id = vm.IdCita });
    }

    // -------- Cancelar --------
    [HttpPost]
    public async Task<IActionResult> Cancelar(CancelarCitaVM vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _svc.CancelarCita(vm.IdCita, vm.Observaciones);
        TempData["ok"] = "Cita cancelada.";
        return RedirectToAction(nameof(Detalle), new { id = vm.IdCita });
    }
}
