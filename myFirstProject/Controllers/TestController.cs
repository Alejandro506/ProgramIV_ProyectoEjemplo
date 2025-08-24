using Microsoft.AspNetCore.Mvc;
using myFirstProject.Data;
using myFirstProject.Data.Models;

namespace myFirstProject.Controllers
{
    public class TestController : Controller
    {
        private readonly AppDbContext _db;
        public TestController(AppDbContext db) => _db = db;

        // GET /Test
        public IActionResult Index()
        {
            var top = _db.Clientes.Take(5).ToList();
            return Ok(top);
        }
    }
}
