using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.IRepository
{
    public interface IPenaltyRepo
    {

        List<PenaltyViewMode> GetAll();
        Penalty GetByCheckoutsId(int id);
        Penalty GetById(int penaltyId);
        PenaltyViewMode GetAllInformation(int penaltyId);
        List<PenaltyViewMode> GetAllInformationByUserName(string User);
        void save();
        void Insert(Penalty penalty);
    }
}
