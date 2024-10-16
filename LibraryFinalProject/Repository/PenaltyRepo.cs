using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.ViewModel;
using Microsoft.VisualBasic;

namespace LibraryFinalProject.Repository
{
    public class PenaltyRepo : IPenaltyRepo
    {
        ApplicationDbContext Context;
        public PenaltyRepo(ApplicationDbContext db)
        {
            Context = db;
        }


        public List<PenaltyViewMode> GetAll()
        {

            var penalties = from p in Context.Penalties
                            join ch in Context.Checkouts on p.Checkouts_Id equals ch.Id
                            join m in Context.Members on ch.Member_Id equals m.Id
                            join b in Context.Books on ch.Book_Id equals b.Id
                            select new { p, Checkouts_Id = ch.Id, Due_Date = ch.Due_Date, FullName = m.FullName, Title = b.Title };


            List<PenaltyViewMode> penaltyViews = penalties.Select(x => new PenaltyViewMode
            {
                Id = x.p.Id,
                TotalDelayDays = x.p.TotalDelayDays,
                TotalPenaltyAmount = x.p.TotalPenaltyAmount,
                FullName = x.FullName,
                Is_Paid = x.p.Is_Paid,
                Checkouts_Id = x.p.Checkouts_Id,
                Due_Date = x.Due_Date,
                Title = x.Title
            }).ToList();
            return penaltyViews;
        }
        public PenaltyViewMode GetAllInformation(int penaltyId)
        {
            PenaltyViewMode penaltyView = new PenaltyViewMode();

            var penalties = from p in Context.Penalties
                            join ch in Context.Checkouts on p.Checkouts_Id equals ch.Id
                            join m in Context.Members on ch.Member_Id equals m.Id
                            join b in Context.Books on ch.Book_Id equals b.Id
                            join r in Context.Returns on ch.Id equals r.Checkouts_Id
                            select new { p, Checkouts_Id = ch.Id, Due_Date = ch.Due_Date, FullName = m.FullName, Title = b.Title, Return_Date = r.Return_Date };
            var penaltyData = penalties.Where(p => p.p.Id == penaltyId).FirstOrDefault();
            if (penaltyData != null)
            {
                penaltyView.Title = penaltyData.Title;
                penaltyView.Due_Date = penaltyData.Due_Date;
                penaltyView.Return_Date = penaltyData.Return_Date;
                penaltyView.FullName = penaltyData.FullName;
                // تعيين باقي الخصائص من penaltyData إذا لزم الأمر
            }

            // تعيين الخصائص التي تم جلبها من Penalty مباشرة إذا لزم الأمر
            var pen = GetById(penaltyId);
            if (pen != null)
            {
                penaltyView.TotalDelayDays = pen.TotalDelayDays;
                penaltyView.TotalPenaltyAmount = pen.TotalPenaltyAmount;
                if (penaltyView.TotalPenaltyAmount == 0)
                {
                    pen.Is_Paid = true;
                    Context.SaveChanges();
                }
                penaltyView.Is_Paid = pen.Is_Paid;
                penaltyView.TotalPenaltyAmount = pen.TotalPenaltyAmount;
                penaltyView.Checkouts_Id = pen.Checkouts_Id;
                penaltyView.Id = pen.Id;
            }

            return penaltyView;
        }

        public List<PenaltyViewMode> GetAllInformationByUserName(string User)
        {
            var penalties = from p in Context.Penalties
                            join ch in Context.Checkouts on p.Checkouts_Id equals ch.Id
                            join m in Context.Members on ch.Member_Id equals m.Id
                            join b in Context.Books on ch.Book_Id equals b.Id
                            join r in Context.Returns on ch.Id equals r.Checkouts_Id
                            where m.UserName == User
                            select new { p, Checkouts_Id = ch.Id, Due_Date = ch.Due_Date, FullName = m.FullName, Title = b.Title, ReturnDate = r.Return_Date };


            List<PenaltyViewMode> penaltyViews = penalties.Select(x => new PenaltyViewMode
            {

                Id = x.p.Id,
                TotalDelayDays = x.p.TotalDelayDays,
                TotalPenaltyAmount = x.p.TotalPenaltyAmount,
                FullName = x.FullName,
                Is_Paid = x.p.Is_Paid,
                Checkouts_Id = x.p.Checkouts_Id,
                Due_Date = x.Due_Date,
                Title = x.Title,
                Return_Date = x.ReturnDate

            }).ToList();
            return penaltyViews;
        }

        public Penalty GetByCheckoutsId(int id)
        {
            return Context.Penalties.FirstOrDefault(P => P.Checkouts_Id == id);
        }

        public Penalty GetById(int penaltyId)
        {
            return Context.Penalties.FirstOrDefault(P => P.Id == penaltyId);
        }

        public void Insert(Penalty penalty)
        {
            Context.Penalties.Add(penalty);
            Context.SaveChanges();
        }

        public void save()
        {
            Context.SaveChanges();
        }
    }
}
