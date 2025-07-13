using myFirstProject.MyInterfaces;
using myFirstProject.MyModels;
using myFirstProject.Data;
using myFirstProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace myFirstProject.MyRepositories
{
    public class SqlLibroRepository : ILibroRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlLibroRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public LibroViewModel? GetById(int id)
        {
            return _context.Libros
                .Where(l => l.LibroId == id)
                .Select(MapToViewModel)
                .FirstOrDefault();
        }

        public IEnumerable<LibroViewModel> GetByFullName(string fullname)
        {
            return _context.Libros
                .Where(l => (l.Titulo + " " + l.Titulo).Contains(fullname)) // Simula FullName
                .Select(MapToViewModel)
                .ToList();
        }

        public IEnumerable<LibroViewModel> GetByYearRange(int fromYear, int toYear)
        {
            return _context.Libros
                .Where(l => l.AnioPublicacion >= fromYear && l.AnioPublicacion <= toYear)
                .Select(MapToViewModel)
                .ToList();
        }

        public IEnumerable<LibroViewModel> GetByEditor(string editor)
        {
            return _context.Libros
                .Where(l => l.Autor == editor)
                .Select(MapToViewModel)
                .ToList();
        }

        public IEnumerable<LibroViewModel> GetAll()
        {
            return _context.Libros
                .Select(MapToViewModel)
                .ToList();
        }

        private LibroViewModel MapToViewModel(Libro libro)
        {
            return new LibroViewModel
            {
                Id = libro.LibroId,
                OriginalName = libro.Titulo,
                SpanishName = libro.Titulo, // Puedes duplicar si no tienes traducción
                Editor = libro.Autor,
                Year = libro.AnioPublicacion,
                Edition = "" // Si no usas esta propiedad aún, puedes dejarla vacía
            };
        }

    }
}
