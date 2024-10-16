using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace LibraryFinalProject.Repository
{
    [Authorize(Roles ="Member")]
    public class MemberRepo : IMemberRepo
    {
        ApplicationDbContext Context;
        public MemberRepo(ApplicationDbContext db)
        {
            Context = db;
        }

        public List<HistoryBookViewModel> historyBookViewModels(string username)
        {
            var MemberID = Context.Members.FirstOrDefault(x => x.UserName.Equals(username));
            var MyHistory = Context.Books.
              Join(
                  Context.Checkouts,
                   book => book.Id,
                  checkout => checkout.Book_Id,
                  (book, checkout) => new
                  {
                      booktitle = book.Title,
                      bookPhoto = book.Book_Photo,
                      checkoutDate = checkout.Checkout_Date,
                      Member_Id=checkout.Member_Id,

                  }
              ) .Where(checkout => checkout.Member_Id == MemberID.Id).
             Join(
                  Context.Members,
                  checkout => checkout.Member_Id,
                  member => member.Id,
                  (checkout, member) => new
                  {
                      booktitle = checkout.booktitle,
                      bookPhoto = checkout.bookPhoto,
                      checkoutDate = checkout.checkoutDate,

                      
                      MemberName = member.FullName
                  }
              );

            List<HistoryBookViewModel> historyBookViewModels = MyHistory.Select(x=>new HistoryBookViewModel()
            {
                Title=x.booktitle,
                Book_Photo=x.bookPhoto,
                FullName = x.MemberName,

                

            }).ToList();
            return historyBookViewModels;


        }

        public List<Member> GetAll()
        {
            return Context.Members.OrderBy(M => M.FullName).ToList();
        }

        public Member GetById(int id)
        {
            return Context.Members.FirstOrDefault(M => M.Id == id);
        }


        public void Insert(RegisterViewModel viewModel)
        {
            Member NewMember = new Member();
            NewMember.Address = viewModel.Address;
            NewMember.Email = viewModel.Email;
            NewMember.FullName = viewModel.FullName;
            NewMember.Phone = viewModel.Phone;
            NewMember.UserName = viewModel.UserName;
            Context.Members.Add(NewMember);
            Context.SaveChanges();
        }

        public void Update(string user, ApplicationUser applicationUser)
        {
            Member member = Context.Members.FirstOrDefault(M => M.UserName == user);
            member.Address = applicationUser.Address;
            member.Email = applicationUser.Email;
            member.FullName = applicationUser.Full_Name;
            member.Phone = applicationUser.PhoneNumber;
            member.UserName = applicationUser.UserName;
            Context.SaveChanges();
        }
    }
}
