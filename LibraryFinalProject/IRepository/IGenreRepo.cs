using LibraryFinalProject.Models;

namespace LibraryFinalProject.IRepository
{
    public interface IGenreRepo
    {
        List<Genre> GetAll();
        void Insert(Genre genre);
        void Update(Genre genre , int id);
        Genre GetById(int id);
    }
}
