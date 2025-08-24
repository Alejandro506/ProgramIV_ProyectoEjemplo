using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models; // AppDbContext + Vista_HistorialCitasDetalle

namespace myFirstProject.Controllers
{
    public class HistorialCitasController : Controller
    {
        private readonly AppDbContext _db;
        public HistorialCitasController(AppDbContext db) => _db = db;

        // /HistorialCitas
        public async Task<IActionResult> Index()
        {
            var datos = await _db.Vista_HistorialCitasDetalles
                                 .OrderByDescending(h => h.FechaAccion)
                                 .ThenByDescending(h => h.FechaHoraCita)
                                 .Take(300) // limita para no traer demasiado
                                 .ToListAsync();
            return View(datos);
        }

        // /HistorialCitas/PorCita/123
        [HttpGet("/HistorialCitas/PorCita/{idCita:int}")]
        public async Task<IActionResult> PorCita(int idCita)
        {
            var datos = await _db.Vista_HistorialCitasDetalles
                                 .Where(h => h.IdCita == idCita)
                                 .OrderByDescending(h => h.FechaAccion)
                                 .ToListAsync();
            return View("Index", datos);
        }

        // /api/historial-citas?idCita=123 (JSON)
        [HttpGet("/api/historial-citas")]
        public async Task<IActionResult> Api([FromQuery] int? idCita)
        {
            var q = _db.Vista_HistorialCitasDetalles.AsQueryable();
            if (idCita.HasValue) q = q.Where(h => h.IdCita == idCita.Value);
            var datos = await q.OrderByDescending(h => h.FechaAccion).ToListAsync();
            return Ok(datos);
        }
    }
}
