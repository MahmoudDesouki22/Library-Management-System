using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.IRepository
{
    public interface ICheckoutsRepo
    {
        List<CheckoutsViewModel> GetAll();
        void insert(Checkouts Checkouts);
        void Delete(int id);
        Checkouts GetById(int id);
        List<Checkouts> GetListByBookId(int id);
        Checkouts GetByBookId(int id);

        List<CheckoutsViewModel> GetAllCheckoutsWithoutReturn();

        List<CheckoutsViewModel> SearchCheckOuts(string search);
    }
}
