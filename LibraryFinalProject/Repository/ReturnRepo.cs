using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.Repository
{
    public class ReturnRepo : IReturnRepo
    {
        ApplicationDbContext Context;
        public ReturnRepo(ApplicationDbContext db)
        {
            Context = db;
        }
        public List<ReturnViewModel> GetAllReturns()
        {
            var returns = Context.Returns.
                Join(
                    Context.Checkouts,
                    returns => returns.Checkouts_Id,
                    checkouts => checkouts.Id,
                    (returns, checkouts) => new
                    {
                        returns,
                        checkouts.Book_Id,
                        checkouts.Member_Id
                    }
                ).Join(
                    Context.Members,
                    returns => returns.Member_Id,
                    member => member.Id,
                    (returns, member) => new
                    {
                        returns,
                        MemberName = member.FullName
                    }
                ).Join(
                    Context.Books,
                    returns => returns.returns.Book_Id,
                    books => books.Id,
                    (returns,books) => new
                    {
                        returns,
                        BookTitle = books.Title
                    }
                );
            List<ReturnViewModel> viewModels = returns.Select(c => new ReturnViewModel
            {
                Id = c.returns.returns.returns.Id,
                Return_Date = c.returns.returns.returns.Return_Date,
                Checkouts_Id = c.returns.returns.returns.Checkouts_Id,
                BookTitle = c.BookTitle,
                MemberName = c.returns.MemberName
            }).ToList();
            return viewModels;
        }

        public void Insert(Return Return)
        {
            try
            {
                Context.Returns.Add(Return);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
            }

        }
        public Return GetByCheckoutsId(int checkoutsId)
        {
            return Context.Returns.FirstOrDefault(r => r.Checkouts_Id == checkoutsId);
        }

    }
}
