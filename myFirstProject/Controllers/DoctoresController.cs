using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;

namespace myFirstProject.Controllers
{
    public class DoctoresController : Controller
    {
        private readonly AppDbContext _db;
        public DoctoresController(AppDbContext db) => _db = db;

        // GET: /Doctores?q=ana
        public async Task<IActionResult> Index(string? q)
        {
            var query = _db.Doctores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim();
                query = query.Where(d => EF.Functions.Like(d.Nombre!, $"%{term}%"));
            }

            var items = await query
                .OrderBy(d => d.Nombre)
                .Select(d => new DoctorRow
                {
                    IdDoctor = d.IdDoctor,
                    Nombre = d.Nombre!,
                    // Total de citas (puedes filtrar por estado/fecha si prefieres)
                    CantCitas = _db.Citas.Count(c => c.IdDoctor == d.IdDoctor)
                })
                .ToListAsync();

            ViewBag.Q = q;
            return View(items);
        }

        // GET: /Doctores/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _db.Doctores.FirstOrDefaultAsync(d => d.IdDoctor == id);
            if (doctor == null) return NotFound();

            var citas = await _db.Citas
                .Include(c => c.IdAnimalNavigation)!.ThenInclude(a => a.IdClienteNavigation)
                .Where(c => c.IdDoctor == id)
                .OrderByDescending(c => c.FechaHora)
                .Take(200)
                .Select(c => new CitaRow
                {
                    IdCita = c.IdCita,
                    FechaHora = c.FechaHora,
                    Estado = c.Estado ?? "",
                    Animal = c.IdAnimalNavigation!.Nombre!,
                    Cliente = c.IdAnimalNavigation!.IdClienteNavigation!.Nombre!
                })
                .ToListAsync();

            ViewBag.Doctor = doctor;
            return View(citas);
        }

        // GET: /Doctores/Create
        public IActionResult Create() => View(new DoctorForm());

        // POST: /Doctores/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorForm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var dup = await _db.Doctores.AnyAsync(d => d.Nombre == vm.Nombre);
            if (dup)
            {
                ModelState.AddModelError(nameof(vm.Nombre), "Ya existe un doctor con ese nombre.");
                return View(vm);
            }

            var entity = new Doctore { Nombre = vm.Nombre };

            // Si IdDoctor NO es identity en DB, descomenta estas líneas:
            // var nextId = (await _db.Doctores.MaxAsync(x => (int?)x.IdDoctor) ?? 0) + 1;
            // entity.IdDoctor = nextId;

            _db.Doctores.Add(entity);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Doctor creado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Doctores/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var d = await _db.Doctores.FindAsync(id);
            if (d == null) return NotFound();

            return View(new DoctorForm { IdDoctor = d.IdDoctor, Nombre = d.Nombre! });
        }

        // POST: /Doctores/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorForm vm)
        {
            if (id != vm.IdDoctor) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var entity = await _db.Doctores.FindAsync(id);
            if (entity == null) return NotFound();

            var dup = await _db.Doctores.AnyAsync(d => d.IdDoctor != id && d.Nombre == vm.Nombre);
            if (dup)
            {
                ModelState.AddModelError(nameof(vm.Nombre), "Ya existe otro doctor con ese nombre.");
                return View(vm);
            }

            entity.Nombre = vm.Nombre;
            await _db.SaveChangesAsync();

            TempData["ok"] = "Doctor actualizado.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Doctores/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var d = await _db.Doctores.FirstOrDefaultAsync(x => x.IdDoctor == id);
            if (d == null) return NotFound();

            var cant = await _db.Citas.CountAsync(c => c.IdDoctor == id);
            ViewBag.CantCitas = cant;
            return View(d);
        }

        // POST: /Doctores/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var d = await _db.Doctores.FindAsync(id);
            if (d == null) return NotFound();

            var cant = await _db.Citas.CountAsync(c => c.IdDoctor == id);
            if (cant > 0)
            {
                TempData["err"] = "No se puede eliminar: el doctor tiene citas registradas.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _db.Doctores.Remove(d);
            await _db.SaveChangesAsync();

            TempData["ok"] = "Doctor eliminado.";
            return RedirectToAction(nameof(Index));
        }

        // ---- VMs locales ----
        public class DoctorRow
        {
            public int IdDoctor { get; set; }
            public string Nombre { get; set; } = "";
            public int CantCitas { get; set; }
        }

        public class CitaRow
        {
            public int IdCita { get; set; }
            public DateTime FechaHora { get; set; }
            public string Estado { get; set; } = "";
            public string Animal { get; set; } = "";
            public string Cliente { get; set; } = "";
        }

        public class DoctorForm
        {
            public int IdDoctor { get; set; }
            [System.ComponentModel.DataAnnotations.Required]
            [System.ComponentModel.DataAnnotations.StringLength(100)]
            public string Nombre { get; set; } = "";
        }
    }
}
