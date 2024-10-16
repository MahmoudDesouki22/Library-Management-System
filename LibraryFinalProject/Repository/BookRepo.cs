using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.ViewModel;
using System.Linq;

namespace LibraryFinalProject.Repository
{
    public class BookRepo : IBookRepo
    {
        ApplicationDbContext Context;
        public BookRepo(ApplicationDbContext db)
        {
            Context = db;
        }

        public List<BookAndGenreViewModel> AllBookAndGenre()
        {
            var Books = Context.Books
                .Join(Context.Genres,
                    book => book.Genre_Id,
                    genre => genre.Id,
                    (book, genre) => new
                    {
                        Book = book,
                        Genre_Name = genre.Name
                    })
                .ToList();

            List<BookAndGenreViewModel> viewModel = Books.Select(c => new BookAndGenreViewModel
            {
                Id = c.Book.Id,
                Title = c.Book.Title,
                Description = c.Book.Description,
                Author = c.Book.Author,
                Availability_Status = c.Book.Availability_Status,
                ISBN = c.Book.ISBN,
                Publish_Date = c.Book.Publish_Date,
                Book_Photo = c.Book.Book_Photo,
                Genre_Name = c.Genre_Name
            }).ToList();

            foreach (var book in viewModel)
            {
                if (book.Availability_Status == "Available" || book.Availability_Status == "available")
                {
                    book.Availability_color = "green";
                }
                else
                {
                    book.Availability_color = "red";
                }
            }
            return viewModel;
        }

        public void Delete(int id)
        {
            Book book = GetById(id);
            Context.Books.Remove(book);
            Context.SaveChanges();
        }

        public List<Book> GetAllBooks()
        {
            return Context.Books.OrderBy(B => B.Title).ToList();
        }

        public List<Genre> GetAllGenre()
        {
            List<Genre> GenreList = Context.Genres.ToList();
            return GenreList;
        }

        public BookAndGenreViewModel GetBookAndGenre(int id)
        {
            Book book = GetById(id);
            BookAndGenreViewModel BookViewModel = new BookAndGenreViewModel();
            BookViewModel.Id = book.Id;
            BookViewModel.Title = book.Title;
            BookViewModel.Description = book.Description;
            BookViewModel.Author = book.Author;
            BookViewModel.Publish_Date = book.Publish_Date;
            BookViewModel.Availability_Status = book.Availability_Status;
            BookViewModel.ISBN = book.ISBN;
            BookViewModel.Book_Photo = book.Book_Photo;
            BookViewModel.Genre_Id = book.Genre_Id;
            Genre genre = Context.Genres.FirstOrDefault(g => g.Id == book.Genre_Id);
            BookViewModel.Genre_Name = genre.Name;
            if (BookViewModel.Availability_Status == "Available")
                BookViewModel.Availability_color = "green";
            else
                BookViewModel.Availability_color = "red";
            return BookViewModel;
        }

        public Book GetById(int id)
        {
            return Context.Books.FirstOrDefault(B => B.Id == id);
        }

        public List<Book> GetNotAvailableBooks()
        {
            return Context.Books.Where(B => B.Availability_Status == "Not Available").OrderBy(B => B.Title).ToList();
        }

        public void Insert(BookAndGenreViewModel BookVM)
        {
            Book NewBook = new Book();
            NewBook.Author = BookVM.Author;
            NewBook.Title = BookVM.Title;
            NewBook.Description = BookVM.Description;
            NewBook.Availability_Status = "Available";
            NewBook.ISBN = BookVM.ISBN;
            NewBook.Book_Photo = BookVM.Book_Photo;
            NewBook.Publish_Date = BookVM.Publish_Date;
            NewBook.Genre_Id = BookVM.Genre_Id;
            NewBook.PricePerWeek = BookVM.PricePerWeek;
            Context.Books.Add(NewBook);
            Context.SaveChanges();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Update(int id, BookAndGenreViewModel BookVM)
        {
            Book OldBook = GetById(id);

            // تحقق مما إذا كانت الصورة الجديدة قد تم تحميلها
            if (!string.IsNullOrEmpty(BookVM.Book_Photo))
            {
                OldBook.Book_Photo = BookVM.Book_Photo; // قم بتحديث الصورة فقط إذا تم تحميل واحدة جديدة
            }

            OldBook.Author = BookVM.Author;
            OldBook.Title = BookVM.Title;
            OldBook.Description = BookVM.Description;
            OldBook.Availability_Status = BookVM.Availability_Status;
            OldBook.ISBN = BookVM.ISBN;
            OldBook.Publish_Date = BookVM.Publish_Date;
            OldBook.Genre_Id = BookVM.Genre_Id;

            Context.SaveChanges();
        }



        //Ahmed Ibrahim
        public List<BookAndGenreViewModel> SearchBooks(string search)
        {
            var result = Context.Books
                .Where(b => b.Title.Contains(search) || b.Author.Contains(search) || b.genre.Name.Contains(search))
                .Select(b => new BookAndGenreViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Availability_Status = b.Availability_Status,
                    Availability_color = b.Availability_Status == "Available" ? "green" : "red",
                    Book_Photo = b.Book_Photo,
                    Genre_Name = b.genre.Name,
                    PricePerWeek = b.PricePerWeek
                }).ToList();

            return result;
        }
    }
}