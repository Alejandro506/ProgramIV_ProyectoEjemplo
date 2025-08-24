using System.Text.Json;
using myFirstProject.MyInterfaces;
using myFirstProject.MyModels;

namespace myFirstProject.MyRepositories
{
    public class JsonLibroRepository : ILibroRepository
    {
        private readonly List<LibroViewModel> _libros;

        public JsonLibroRepository(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException("Archivo JSON no encontrado", jsonPath);

            var json = File.ReadAllText(jsonPath);
            _libros = JsonSerializer.Deserialize<List<LibroViewModel>>(json) ?? new List<LibroViewModel>();
        }

        public LibroViewModel? GetById(int id) =>
            _libros.FirstOrDefault(l => l.Id == id);

        public IEnumerable<LibroViewModel> GetByFullName(string fullname) =>
            _libros.Where(l => l.FullName.Contains(fullname, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<LibroViewModel> GetByYearRange(int desde, int hasta) =>
            _libros.Where(l => l.Year >= desde && l.Year <= hasta);

        public IEnumerable<LibroViewModel> GetByEditor(string editor) =>
            _libros.Where(l => l.Editor.Equals(editor, StringComparison.OrdinalIgnoreCase));

        public IEnumerable<LibroViewModel> GetAll() => _libros;
    }
}
