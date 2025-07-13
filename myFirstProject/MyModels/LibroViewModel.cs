namespace myFirstProject.MyModels
{
    public class LibroViewModel
    {
        public int Id { get; set; }
        public string OriginalName { get; set; } = string.Empty;
        public string SpanishName { get; set; } = string.Empty;
        public string Edition { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Editor { get; set; } = string.Empty;

        public string FullName => $"{OriginalName} / {SpanishName}";
        public int Antiquity => DateTime.Now.Year - Year;
    }
}

