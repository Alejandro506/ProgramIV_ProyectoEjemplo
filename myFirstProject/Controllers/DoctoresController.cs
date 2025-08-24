using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Controllers;

public class DoctoresController : Controller
{
    private readonly AppDbContext _db;
    public DoctoresController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.Doctores.OrderBy(d => d.Nombre).ToListAsync());

    public async Task<IActionResult> Detalle(int id)
    {
        var doc = await _db.Doctores
            .Include(d => d.HorariosDoctors)      // horarios semanales
            .FirstOrDefaultAsync(d => d.IdDoctor == id);
        return doc == null ? NotFound() : View(doc);
    }

    public IActionResult Crear() => View(new Doctore());
    [HttpPost]
    public async Task<IActionResult> Crear(Doctore vm)
    {
        if (!ModelState.IsValid) return View(vm);
        _db.Doctores.Add(vm); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var d = await _db.Doctores.FindAsync(id);
        return d == null ? NotFound() : View(d);
    }
    [HttpPost]
    public async Task<IActionResult> Editar(int id, Doctore vm)
    {
        if (id != vm.IdDoctor) return BadRequest();
        if (!ModelState.IsValid) return View(vm);
        _db.Update(vm); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Detalle), new { id });
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var d = await _db.Doctores.FindAsync(id);
        return d == null ? NotFound() : View(d);
    }
    [HttpPost, ActionName("Eliminar")]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var d = await _db.Doctores.FindAsync(id);
        if (d != null) { _db.Remove(d); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    // --- Horarios semanales ---
    public async Task<IActionResult> NuevoHorario(int idDoctor)
    {
        return View(new HorariosDoctor { IdDoctor = idDoctor });
    }
    [HttpPost]
    public async Task<IActionResult> NuevoHorario(HorariosDoctor vm)
    {
        if (!ModelState.IsValid) return View(vm);
        _db.HorariosDoctors.Add(vm);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Detalle), new { id = vm.IdDoctor });
    }

    public async Task<IActionResult> EliminarHorario(int id) // idHorario
    {
        var h = await _db.HorariosDoctors.FindAsync(id);
        if (h == null) return NotFound();
        var idDoctor = h.IdDoctor;
        _db.Remove(h); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Detalle), new { id = idDoctor });
    }
}
