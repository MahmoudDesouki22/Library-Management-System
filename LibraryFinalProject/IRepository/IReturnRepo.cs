using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.IRepository
{
    public interface IReturnRepo
    {
        List<ReturnViewModel> GetAllReturns();
        void Insert(Return Return);
        Return GetByCheckoutsId(int checkoutsId);
    }
}
