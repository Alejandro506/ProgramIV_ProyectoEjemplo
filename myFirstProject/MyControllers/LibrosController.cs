using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using myFirstProject.MyInterfaces;
using myFirstProject.MyModels;

namespace myFirstProject.MyControllers
{
    public class LibrosController : Controller
    {
        private readonly ILibroRepository _repo;
        private readonly IConfiguration _config;

        public LibrosController(ILibroRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // ✅ Acción por defecto que carga todos los libros
        public IActionResult Buscar()
        {
            var libros = _repo.GetAll();
            return View("Lista", libros); // Reutiliza la vista Lista.cshtml
        }

        public IActionResult PorId(int id)
        {
            var libro = _repo.GetById(id);
            if (libro == null)
                return NotFound("Libro no encontrado");

            return View("Mostrar", libro);
        }

        public IActionResult PorEditor(string editor)
        {
            var libros = _repo.GetByEditor(editor);
            return View("Lista", libros);
        }

        public IActionResult PorFullName(string fullname)
        {
            var libros = _repo.GetByFullName(fullname);
            return View("Lista", libros);
        }

        public IActionResult PorRango(int desde, int hasta)
        {
            var libros = _repo.GetByYearRange(desde, hasta);
            return View("Lista", libros);
        }
    }
}
