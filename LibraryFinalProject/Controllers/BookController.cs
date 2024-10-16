using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;
using LibraryFinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryFinalProject.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        IBookRepo BookRepository;

        public BookController(IBookRepo BookRepo)
        {
            BookRepository = BookRepo;
        }

        public IActionResult Index(string search)
        {
            List<BookAndGenreViewModel> BookList;
            if (string.IsNullOrEmpty(search))
            {
                BookList = BookRepository.AllBookAndGenre(); 
            }
            else
            {
                BookList = BookRepository.SearchBooks(search);
                ViewData["SearchQuery"] = search;
            }
            return View(BookList);
        }

        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult IndexForManagers(string search)
        {
            List<BookAndGenreViewModel> BookList;
            if (string.IsNullOrEmpty(search))
            {
                BookList = BookRepository.AllBookAndGenre();
            }
            else
            {
                BookList = BookRepository.SearchBooks(search); 
                ViewData["SearchQuery"] = search;
            }
            return View(BookList);
        }

        public IActionResult Details(int id)
        {
            BookAndGenreViewModel BookViewModel = BookRepository.GetBookAndGenre(id);
            return View(BookViewModel);
        }

        [Authorize(Roles = "Admin,Librarian")]
        [HttpGet]
        public IActionResult AddBook()
        {
            BookAndGenreViewModel bookViewModel = new BookAndGenreViewModel();
            bookViewModel.genres = BookRepository.GetAllGenre();
            return View(bookViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(BookAndGenreViewModel NewBookVM)
        {
            if (ModelState.IsValid)
            {
                if (NewBookVM.BookPhotoFile != null)
                {
                    // إنشاء اسم فريد للملف وحفظه
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(NewBookVM.BookPhotoFile.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        NewBookVM.BookPhotoFile.CopyTo(fileStream);
                    }

                    // تعيين مسار الملف في ViewModel
                    NewBookVM.Book_Photo = uniqueFileName;
                }

                BookRepository.Insert(NewBookVM);
                return RedirectToAction("IndexForManagers");
            }

            NewBookVM.genres = BookRepository.GetAllGenre();
            return View(NewBookVM);
        }




        [HttpGet]
        [Authorize(Roles = "Admin,Librarian")]
        public IActionResult EditBook(int id)
        {
            if (id == 0)
                return Content("Error Insert id");
            BookAndGenreViewModel BookViewModel = new BookAndGenreViewModel();
            Book Book = BookRepository.GetById(id);
            BookViewModel.Id = Book.Id;
            BookViewModel.Author = Book.Author;
            BookViewModel.Description = Book.Description;
            BookViewModel.ISBN = Book.ISBN;
            BookViewModel.Book_Photo = Book.Book_Photo;
            BookViewModel.Publish_Date = Book.Publish_Date;
            BookViewModel.Availability_Status = Book.Availability_Status;
            BookViewModel.Genre_Id = Book.Genre_Id;
            BookViewModel.Title = Book.Title;
            BookViewModel.PricePerWeek = Book.PricePerWeek;
            BookViewModel.genres = BookRepository.GetAllGenre();
            return View(BookViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBook(int id, BookAndGenreViewModel BookVM)
        {
            if (ModelState.IsValid)
            {
                // الحصول على الكتاب الحالي من قاعدة البيانات
                var existingBook = BookRepository.GetById(id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                // تحديث الحقول الأخرى
                existingBook.Author = BookVM.Author;
                existingBook.Title = BookVM.Title;
                existingBook.Description = BookVM.Description;
                existingBook.Availability_Status = BookVM.Availability_Status;
                existingBook.ISBN = BookVM.ISBN;
                existingBook.PricePerWeek = BookVM.PricePerWeek;
                existingBook.Publish_Date = BookVM.Publish_Date;
                existingBook.Genre_Id = BookVM.Genre_Id;

                // التعامل مع رفع الصورة
                if (BookVM.BookPhotoFile != null && BookVM.BookPhotoFile.Length > 0)
                {
                    // إنشاء اسم فريد للملف وحفظه
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(BookVM.BookPhotoFile.FileName);
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // حفظ الصورة الجديدة في المجلد
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        BookVM.BookPhotoFile.CopyTo(fileStream);
                    }

                    // تعيين مسار الصورة الجديدة في الكيان
                    existingBook.Book_Photo = uniqueFileName; // تحديث الصورة الجديدة
                }

                // حفظ التغييرات في قاعدة البيانات
                BookRepository.Update(id, BookVM);
                return RedirectToAction("IndexForManagers");
            }

            // في حالة وجود خطأ في التحقق من الصحة، إعادة تحميل الأنواع
            BookVM.genres = BookRepository.GetAllGenre();
            return View(BookVM);
        }




        public IActionResult DeleteBook(int id)
        {
            BookRepository.Delete(id);
            return RedirectToAction("IndexForManagers");
        }
    }
}