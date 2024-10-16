using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.ViewModel;

namespace LibraryFinalProject.Repository
{
    public class CheckoutsRepo : ICheckoutsRepo
    {
        ApplicationDbContext Context;
        public CheckoutsRepo(ApplicationDbContext db)
        {
            Context = db;
        }

        public void Delete(int id)
        {
            Checkouts checkouts = GetById(id);
            //Book book = Context.Books.FirstOrDefault(x => x.Id == checkouts.Book_Id);
            //book.Availability_Status = "Available";
            Context.Checkouts.Remove(checkouts);
            Context.SaveChanges();
        }

        public List<CheckoutsViewModel> GetAll()
        {
            int count = 1;
            var checkouts = Context.Checkouts.
                Join(
                    Context.Books,
                    checkout => checkout.Book_Id,
                    book => book.Id,
                    (checkout, book) => new
                    {
                        checkout,
                        BookTitle = book.Title,
                        book.PricePerWeek
                    }
                ).Join(
                    Context.Members,
                    checkout => checkout.checkout.Member_Id,
                    member => member.Id,
                    (checkout,member) => new
                    {
                        checkout,
                        MemberName = member.FullName
                    }
                );
            List<CheckoutsViewModel> viewModels = checkouts.Select(c => new CheckoutsViewModel
            {
                Id = c.checkout.checkout.Id,
                Checkout_Date = c.checkout.checkout.Checkout_Date,
                Due_Date = c.checkout.checkout.Due_Date,
                BookTitle = c.checkout.BookTitle,
                Book_Id = c.checkout.checkout.Book_Id,
                Librarian_Id = c.checkout.checkout.Librarian_Id,
                Member_Id = c.checkout.checkout.Member_Id,
                MemberName = c.MemberName,
                PricePerWeek = c.checkout.PricePerWeek
            }).ToList();
            foreach (var model in viewModels) 
            {
                TimeSpan difference = model.Due_Date - model.Checkout_Date;
                count = (int)Math.Ceiling((double)difference.Days / 7);
                model.TotalPrice = model.PricePerWeek * count;
                count = 1;
            }
            return viewModels;
        }

        public List<CheckoutsViewModel> GetAllCheckoutsWithoutReturn()
        {
            int count = 1;

            // الحصول على السجلات من Checkouts مع الكتب والأعضاء
            var checkoutsWithoutReturn = Context.Checkouts
                .Join(
                    Context.Books,
                    checkout => checkout.Book_Id,
                    book => book.Id,
                    (checkout, book) => new
                    {
                        checkout,
                        BookTitle = book.Title,
                        book.PricePerWeek
                    }
                )
                .Join(
                    Context.Members,
                    checkout => checkout.checkout.Member_Id,
                    member => member.Id,
                    (checkout, member) => new
                    {
                        checkout,
                        MemberName = member.FullName
                    }
                )
                // عمل GroupJoin مع جدول Returns للتحقق من الكتب التي تم إرجاعها
                .GroupJoin(
                    Context.Returns,
                    checkout => checkout.checkout.checkout.Id,  // الربط باستخدام Id من Checkouts
                    returnItem => returnItem.Checkouts_Id,  // الربط باستخدام Checkouts_Id من جدول Returns
                    (checkout, returnGroup) => new { checkout, returnGroup }
                )
                // استخدام Left Join لضمان الحصول على جميع السجلات حتى التي لا يوجد لها إرجاع
                .SelectMany(
                    x => x.returnGroup.DefaultIfEmpty(),
                    (x, returnRecord) => new { x.checkout, returnRecord }
                )
                // الفلترة: إظهار فقط السجلات التي لا يوجد لها سجل إرجاع
                .Where(x => x.returnRecord == null)
                .Select(x => x.checkout)  // إرجاع بيانات Checkouts فقط
                .ToList();

            // تحويل النتائج إلى ViewModels
            List<CheckoutsViewModel> viewModels = checkoutsWithoutReturn.Select(c => new CheckoutsViewModel
            {
                Id = c.checkout.checkout.Id,
                Checkout_Date = c.checkout.checkout.Checkout_Date,
                Due_Date = c.checkout.checkout.Due_Date,
                BookTitle = c.checkout.BookTitle,
                Book_Id = c.checkout.checkout.Book_Id,
                Librarian_Id = c.checkout.checkout.Librarian_Id,
                Member_Id = c.checkout.checkout.Member_Id,
                MemberName = c.MemberName,
                PricePerWeek = c.checkout.PricePerWeek
            }).ToList();

            // حساب السعر الإجمالي لكل عملية استعارة بناءً على المدة
            foreach (var model in viewModels)
            {
                TimeSpan difference = model.Due_Date - model.Checkout_Date;
                count = (int)Math.Ceiling((double)difference.Days / 7);
                model.TotalPrice = model.PricePerWeek * count;
                count = 1;
            }

            return viewModels;
        }


        public List<Checkouts> GetListByBookId(int BookId)
        {
            return Context.Checkouts.Where(C => C.Book_Id == BookId).ToList();
        }


        public Checkouts GetById(int id)
        {
            return Context.Checkouts.FirstOrDefault(C => C.Id == id);
        }

        public void insert(Checkouts Checkouts)
        {
            Context.Checkouts.Add(Checkouts);
            Context.SaveChanges();
        }

        public Checkouts GetByBookId(int id)
        {
            return Context.Checkouts.FirstOrDefault(C => C.Book_Id == id);
        }

        public List<CheckoutsViewModel> SearchCheckOuts(string search)
        {
            var result = Context.Checkouts
                .Where(b => b.member.FullName.Contains(search) || b.book.Title.Contains(search))
                .Select(b => new CheckoutsViewModel
                {
                    Id = b.Id,
                    MemberName = b.member.FullName,
                    BookTitle = b.book.Title
                }).ToList();

            return result;
        }
    }
}
