using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace myFirstProject.Controllers
{
    public class TiposAnimalesController : Controller
    {
        private readonly AppDbContext _db;
        public TiposAnimalesController(AppDbContext db) => _db = db;

        // GET: /TiposAnimales?q=perro
        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.TiposAnimales.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(t => EF.Functions.Like(t.Descripcion!, $"%{term}%"));
            }

            var items = await query
                .OrderBy(t => t.Descripcion)
                .Select(t => new TipoAnimalRow
                {
                    IdTipo = t.IdTipo,
                    Descripcion = t.Descripcion!,
                    CantAnimales = _db.Animales.Count(a => a.IdTipo == t.IdTipo)
                })
                .ToListAsync();

            ViewBag.Q = q;
            return View(items);
        }

        // GET: /TiposAnimales/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var tipo = await _db.TiposAnimales.FirstOrDefaultAsync(t => t.IdTipo == id);
            if (tipo == null) return NotFound();

            var animales = await _db.Animales
                .Include(a => a.IdClienteNavigation)
                .Where(a => a.IdTipo == id)
                .OrderBy(a => a.Nombre)
                .Select(a => new AnimalOfTypeRow
                {
                    IdAnimal = a.IdAnimal,
                    Nombre = a.Nombre!,
                    Cliente = a.IdClienteNavigation!.Nombre!
                })
                .ToListAsync();

            ViewBag.Tipo = tipo;
            return View(animales);
        }

        // GET: /TiposAnimales/Create
        public IActionResult Create() => View(new TipoAnimalForm { Descripcion = "" });

        // POST: /TiposAnimales/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TipoAnimalForm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var exists = await _db.TiposAnimales.AnyAsync(t => t.Descripcion == vm.Descripcion);
            if (exists)
            {
                ModelState.AddModelError(nameof(vm.Descripcion), "Ya existe un tipo con esa descripción.");
                return View(vm);
            }

            // Como IdTipo no es IDENTITY, calculamos el siguiente Id manualmente
            var nextId = (await _db.TiposAnimales.MaxAsync(t => (int?)t.IdTipo) ?? 0) + 1;

            var entity = new TiposAnimale
            {
                IdTipo = nextId,
                Descripcion = vm.Descripcion
            };

            _db.TiposAnimales.Add(entity);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Tipo de animal creado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /TiposAnimales/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var tipo = await _db.TiposAnimales.FindAsync(id);
            if (tipo == null) return NotFound();
            return View(new TipoAnimalForm { IdTipo = tipo.IdTipo, Descripcion = tipo.Descripcion! });
        }

        // POST: /TiposAnimales/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoAnimalForm vm)
        {
            if (id != vm.IdTipo) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var entity = await _db.TiposAnimales.FindAsync(id);
            if (entity == null) return NotFound();

            var duplicate = await _db.TiposAnimales
                .AnyAsync(t => t.IdTipo != id && t.Descripcion == vm.Descripcion);
            if (duplicate)
            {
                ModelState.AddModelError(nameof(vm.Descripcion), "Ya existe otro tipo con esa descripción.");
                return View(vm);
            }

            entity.Descripcion = vm.Descripcion;
            await _db.SaveChangesAsync();

            TempData["ok"] = "Tipo de animal actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /TiposAnimales/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var tipo = await _db.TiposAnimales.FirstOrDefaultAsync(t => t.IdTipo == id);
            if (tipo == null) return NotFound();

            var count = await _db.Animales.CountAsync(a => a.IdTipo == id);
            ViewBag.CantAnimales = count;
            return View(tipo);
        }

        // POST: /TiposAnimales/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tipo = await _db.TiposAnimales.FindAsync(id);
            if (tipo == null) return NotFound();

            var count = await _db.Animales.CountAsync(a => a.IdTipo == id);
            if (count > 0)
            {
                TempData["err"] = "No se puede eliminar porque existen animales de este tipo.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _db.TiposAnimales.Remove(tipo);
            await _db.SaveChangesAsync();
            TempData["ok"] = "Tipo de animal eliminado.";
            return RedirectToAction(nameof(Index));
        }

        // --- VMs locales ---
        public class TipoAnimalRow
        {
            public int IdTipo { get; set; }
            public string Descripcion { get; set; } = "";
            public int CantAnimales { get; set; }
        }

        public class AnimalOfTypeRow
        {
            public int IdAnimal { get; set; }
            public string Nombre { get; set; } = "";
            public string Cliente { get; set; } = "";
        }

        public class TipoAnimalForm
        {
            public int IdTipo { get; set; }

            [Required, StringLength(50)]
            public string Descripcion { get; set; } = "";
        }
    }
}
