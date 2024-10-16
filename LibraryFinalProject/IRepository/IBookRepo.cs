using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.IRepository
{
    public interface IBookRepo
    {
        List<BookAndGenreViewModel> AllBookAndGenre();
        BookAndGenreViewModel GetBookAndGenre(int id);
        void Insert(BookAndGenreViewModel BookVM);
        void Update(int id, BookAndGenreViewModel BookVM);
        Book GetById(int id);
        void Delete(int id);
        List<Genre> GetAllGenre();
        List<Book> GetAllBooks();
        void Save();
        List<Book> GetNotAvailableBooks();

        // إضافة الدالة الجديدة
        List<BookAndGenreViewModel> SearchBooks(string search);
    }
}