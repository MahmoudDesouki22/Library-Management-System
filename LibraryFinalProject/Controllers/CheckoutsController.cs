using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryFinalProject.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class CheckoutsController : Controller
    {
        ICheckoutsRepo CheckoutsRepository;
        ILibrarianRepo LibrarianRepository;
        IMemberRepo MemberRepository;
        IBookRepo BookRepository;
        public CheckoutsController(ICheckoutsRepo checkoutsRepo , IMemberRepo memberRepo , ILibrarianRepo librarianRepo , IBookRepo bookRepo)
        {
            CheckoutsRepository = checkoutsRepo;
            MemberRepository = memberRepo;
            LibrarianRepository = librarianRepo;
            BookRepository = bookRepo;
        }
        public IActionResult Index(string search)
        {
            List<CheckoutsViewModel> Checkoutslist = CheckoutsRepository.GetAll();
            ViewData["SearchQuery"] = search;
            return View(Checkoutslist);
        }
        [HttpGet]
        public IActionResult Add()
        {
            CheckoutsViewModel model = new CheckoutsViewModel();
            model.members = MemberRepository.GetAll();
            model.Librarians = LibrarianRepository.GetAll();
            model.books = BookRepository.GetAllBooks();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(CheckoutsViewModel CheckoutsVM)
        {
            if (ModelState.IsValid)
            {
                Book book = BookRepository.GetById(CheckoutsVM.Book_Id);
                if (book.Availability_Status == "Not Available")
                {
                    ModelState.AddModelError("", "The Book is not Available");
                    CheckoutsVM.members = MemberRepository.GetAll();
                    CheckoutsVM.Librarians = LibrarianRepository.GetAll();
                    CheckoutsVM.books = BookRepository.GetAllBooks();
                    return View(CheckoutsVM);
                }
                book.Availability_Status = "Not Available";
                BookRepository.Save();
                Checkouts checkouts = new Checkouts();
                checkouts.Member_Id = CheckoutsVM.Member_Id;
                checkouts.Book_Id = CheckoutsVM.Book_Id;
                checkouts.Checkout_Date = CheckoutsVM.Checkout_Date;
                checkouts.Due_Date = CheckoutsVM.Due_Date;
                checkouts.Librarian_Id = CheckoutsVM.Librarian_Id;
                CheckoutsRepository.insert(checkouts);
                return RedirectToAction("Index");
            }
            CheckoutsVM.members = MemberRepository.GetAll();
            CheckoutsVM.Librarians = LibrarianRepository.GetAll();
            CheckoutsVM.books = BookRepository.GetAllBooks();
            return View(CheckoutsVM);
        }
        public IActionResult Delete(int id)
        {
            CheckoutsRepository.Delete(id);
            return RedirectToAction("Index");
        }
        public IActionResult IndexNotReturn()
        {
            List<CheckoutsViewModel> Checkoutslist = CheckoutsRepository.GetAllCheckoutsWithoutReturn();
            return View(Checkoutslist);
        }
    }
}
