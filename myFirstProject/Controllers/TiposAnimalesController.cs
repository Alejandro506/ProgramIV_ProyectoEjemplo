using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Controllers;

public class TiposAnimalesController : Controller
{
    private readonly AppDbContext _db;
    public TiposAnimalesController(AppDbContext db) => _db = db;

    // GET: /TiposAnimales
    public async Task<IActionResult> Index()
    {
        var tipos = await _db.TiposAnimales
            .OrderBy(t => t.Descripcion)
            .ToListAsync();
        return View(tipos);
    }

    // GET: /TiposAnimales/Detalle/5
    public async Task<IActionResult> Detalle(int id)
    {
        var tipo = await _db.TiposAnimales
            .Include(t => t.Animales)           // detalle de animales por tipo
            .ThenInclude(a => a.IdClienteNavigation)
            .FirstOrDefaultAsync(t => t.IdTipo == id);

        if (tipo == null) return NotFound();
        return View(tipo);
    }

    public IActionResult Crear() => View(new TiposAnimale());

    [HttpPost]
    public async Task<IActionResult> Crear(TiposAnimale vm)
    {
        if (!ModelState.IsValid) return View(vm);
        _db.Add(vm);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var tipo = await _db.TiposAnimales.FindAsync(id);
        return tipo == null ? NotFound() : View(tipo);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(int id, TiposAnimale vm)
    {
        if (id != vm.IdTipo) return BadRequest();
        if (!ModelState.IsValid) return View(vm);
        _db.Update(vm);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var tipo = await _db.TiposAnimales.FindAsync(id);
        return tipo == null ? NotFound() : View(tipo);
    }

    [HttpPost, ActionName("Eliminar")]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var tipo = await _db.TiposAnimales.FindAsync(id);
        if (tipo != null) { _db.Remove(tipo); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }
}
