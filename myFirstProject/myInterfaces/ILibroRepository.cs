using myFirstProject.MyModels;
using System.Collections.Generic;

namespace myFirstProject.MyInterfaces
{
    public interface ILibroRepository
    {
        LibroViewModel? GetById(int id);
        IEnumerable<LibroViewModel> GetByFullName(string fullname);
        IEnumerable<LibroViewModel> GetByYearRange(int fromYear, int toYear);
        IEnumerable<LibroViewModel> GetByEditor(string editor);
        IEnumerable<LibroViewModel> GetAll();
    }
}
