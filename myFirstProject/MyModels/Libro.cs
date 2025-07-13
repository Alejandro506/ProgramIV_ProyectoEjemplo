namespace myFirstProject.Models
{
    public class Libro
    {
        public int LibroId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
    }
}
