using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.IRepository
{
    public interface IMemberRepo
    {
        void Insert(RegisterViewModel viewModel);
        List<Member> GetAll();
        Member GetById(int id);
        void Update(string user, ApplicationUser applicationUser);

        List<HistoryBookViewModel> historyBookViewModels(string Username);


        //int GetId(BookAndGenreViewModel bookAndGenreView);
    }
}
