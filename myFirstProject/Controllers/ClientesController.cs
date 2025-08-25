using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Controllers
{
    public class ClientesController : Controller
    {
        private readonly AppDbContext _db;
        public ClientesController(AppDbContext db) => _db = db;

        // GET: /Clientes?q=ana
        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.Clientes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(c => EF.Functions.Like(c.Nombre!, $"%{term}%"));
            }

            var items = await query
                .OrderBy(c => c.Nombre)
                .Select(c => new ClienteRow
                {
                    IdCliente = c.IdCliente,
                    Nombre = c.Nombre!,
                    CantAnimales = _db.Animales.Count(a => a.IdCliente == c.IdCliente)
                })
                .ToListAsync();

            ViewBag.Q = q;
            return View(items);
        }

        // GET: /Clientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var cliente = await _db.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);
            if (cliente == null) return NotFound();

            var animales = await _db.Animales
                .Include(a => a.IdTipoNavigation)
                .Where(a => a.IdCliente == id)
                .OrderBy(a => a.Nombre)
                .Select(a => new AnimalRow
                {
                    IdAnimal = a.IdAnimal,
                    Nombre = a.Nombre!,
                    Tipo = a.IdTipoNavigation != null ? a.IdTipoNavigation.Descripcion! : "(sin tipo)"
                })
                .ToListAsync();

            ViewBag.Cliente = cliente;
            return View(animales);
        }

        // GET: /Clientes/Create
        public IActionResult Create() => View(new ClienteForm());

        // POST: /Clientes/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteForm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dup = await _db.Clientes.AnyAsync(c => c.Nombre == vm.Nombre);
            if (dup)
            {
                ModelState.AddModelError(nameof(vm.Nombre), "Ya existe un cliente con ese nombre.");
                return View(vm);
            }

            var entity = new Cliente { Nombre = vm.Nombre };

            // Si tu PK NO es identity, descomenta estas dos líneas (igual que hicimos con TiposAnimales):
            // var nextId = (await _db.Clientes.MaxAsync(x => (int?)x.IdCliente) ?? 0) + 1;
            // entity.IdCliente = nextId;

            _db.Clientes.Add(entity);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Cliente creado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Clientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _db.Clientes.FindAsync(id);
            if (c == null) return NotFound();

            return View(new ClienteForm { IdCliente = c.IdCliente, Nombre = c.Nombre! });
        }

        // POST: /Clientes/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteForm vm)
        {
            if (id != vm.IdCliente) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var entity = await _db.Clientes.FindAsync(id);
            if (entity == null) return NotFound();

            var dup = await _db.Clientes.AnyAsync(c => c.IdCliente != id && c.Nombre == vm.Nombre);
            if (dup)
            {
                ModelState.AddModelError(nameof(vm.Nombre), "Ya existe otro cliente con ese nombre.");
                return View(vm);
            }

            entity.Nombre = vm.Nombre;
            await _db.SaveChangesAsync();

            TempData["ok"] = "Cliente actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Clientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Clientes.FirstOrDefaultAsync(x => x.IdCliente == id);
            if (c == null) return NotFound();

            var cant = await _db.Animales.CountAsync(a => a.IdCliente == id);
            ViewBag.CantAnimales = cant;
            return View(c);
        }

        // POST: /Clientes/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var c = await _db.Clientes.FindAsync(id);
            if (c == null) return NotFound();

            var cant = await _db.Animales.CountAsync(a => a.IdCliente == id);
            if (cant > 0)
            {
                TempData["err"] = "No se puede eliminar: el cliente tiene animales registrados.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _db.Clientes.Remove(c);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Cliente eliminado.";
            return RedirectToAction(nameof(Index));
        }

        // ---- VMs locales (simples) ----
        public class ClienteRow
        {
            public int IdCliente { get; set; }
            public string Nombre { get; set; } = "";
            public int CantAnimales { get; set; }
        }

        public class AnimalRow
        {
            public int IdAnimal { get; set; }
            public string Nombre { get; set; } = "";
            public string Tipo { get; set; } = "";
        }

        public class ClienteForm
        {
            public int IdCliente { get; set; }
            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.StringLength(100)]
            public string Nombre { get; set; } = "";
        }
    }
}
