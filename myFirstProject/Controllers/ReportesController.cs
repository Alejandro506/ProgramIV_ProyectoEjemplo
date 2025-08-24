using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myFirstProject.Data.Models;   // AppDbContext + Vista_Animales_Cita

namespace myFirstProject.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _db;
        public ReportesController(AppDbContext db) => _db = db;

        // Vista Razor: /Reportes/AnimalesConCitas
        public async Task<IActionResult> AnimalesConCitas()
{
    var datos = await _db.Vista_Animales_Citas
                         .OrderBy(v => v.NombreAnimal)
                         .ThenBy(v => v.FechaHora)
                         .ToListAsync();
    return View(datos);
}

        [HttpGet("/api/reportes/animales-con-citas")]
        public async Task<IActionResult> AnimalesConCitasApi()
        {
            var datos = await _db.Vista_Animales_Citas.ToListAsync();
            return Ok(datos);
        }
    }
}
