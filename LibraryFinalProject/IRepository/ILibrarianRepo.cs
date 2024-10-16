using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.IRepository
{
    public interface ILibrarianRepo
    {
        void Insert(RegisterViewModel viewModel);
        List<Librarian> GetAll();
        void Update(string user,ApplicationUser applicationUser);
    }
}
