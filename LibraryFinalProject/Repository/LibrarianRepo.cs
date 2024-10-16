using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.Repository
{
    public class LibrarianRepo : ILibrarianRepo
    {
        ApplicationDbContext Context;
        public LibrarianRepo(ApplicationDbContext db)
        {
            Context = db;
        }

        public List<Librarian> GetAll()
        {
            return Context.Librarians.OrderBy(L => L.FullName).ToList();
        }

        public void Insert(RegisterViewModel viewModel)
        {
            Librarian NewLibrarian = new Librarian();
            NewLibrarian.Address = viewModel.Address;
            NewLibrarian.Email = viewModel.Email;
            NewLibrarian.FullName = viewModel.FullName;
            NewLibrarian.Phone = viewModel.Phone;
            NewLibrarian.UserName = viewModel.UserName;
            Context.Librarians.Add(NewLibrarian);
            Context.SaveChanges();
        }

        public void Update(string user, ApplicationUser applicationUser)
        {
            Librarian librarian = Context.Librarians.FirstOrDefault(L => L.UserName == user);
            librarian.Address = applicationUser.Address;
            librarian.Email = applicationUser.Email;
            librarian.FullName = applicationUser.Full_Name;
            librarian.Phone = applicationUser.PhoneNumber;
            librarian.UserName = applicationUser.UserName;
            Context.SaveChanges();
        }
    }
}
