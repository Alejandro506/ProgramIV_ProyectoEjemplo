using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Controllers;

public class ClientesController : Controller
{
    private readonly AppDbContext _db;
    public ClientesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? q)
    {
        var query = _db.Clientes.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(c => EF.Functions.Like(c.Nombre, $"%{q}%"));
        return View(await query.OrderBy(c => c.Nombre).ToListAsync());
    }

    public async Task<IActionResult> Detalle(int id)
    {
        var cli = await _db.Clientes
            .Include(c => c.Animales).ThenInclude(a => a.IdTipoNavigation)
            .FirstOrDefaultAsync(c => c.IdCliente == id);
        return cli == null ? NotFound() : View(cli);
    }

    public IActionResult Crear() => View(new Cliente());
    [HttpPost]
    public async Task<IActionResult> Crear(Cliente vm)
    {
        if (!ModelState.IsValid) return View(vm);
        _db.Add(vm); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var cli = await _db.Clientes.FindAsync(id);
        return cli == null ? NotFound() : View(cli);
    }
    [HttpPost]
    public async Task<IActionResult> Editar(int id, Cliente vm)
    {
        if (id != vm.IdCliente) return BadRequest();
        if (!ModelState.IsValid) return View(vm);
        _db.Update(vm); await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Detalle), new { id });
    }

    public async Task<IActionResult> Eliminar(int id)
    {
        var cli = await _db.Clientes.FindAsync(id);
        return cli == null ? NotFound() : View(cli);
    }
    [HttpPost, ActionName("Eliminar")]
    public async Task<IActionResult> EliminarConfirmado(int id)
    {
        var cli = await _db.Clientes.FindAsync(id);
        if (cli != null) { _db.Remove(cli); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    // --- Animales del cliente ---
    public async Task<IActionResult> NuevoAnimal(int idCliente)
    {
        ViewBag.Tipos = await _db.TiposAnimales.OrderBy(t => t.Descripcion).ToListAsync();
        return View(new Animale { IdCliente = idCliente });
    }
    [HttpPost]
    public async Task<IActionResult> NuevoAnimal(Animale vm)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Tipos = await _db.TiposAnimales.ToListAsync();
            return View(vm);
        }
        _db.Animales.Add(vm);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Detalle), new { id = vm.IdCliente });
    }
}
