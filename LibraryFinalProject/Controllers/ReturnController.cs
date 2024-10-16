using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryFinalProject.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class ReturnController : Controller
    {
        IReturnRepo ReturnRepository;
        IBookRepo BookRepository;
        IMemberRepo MemberRepository;
        ICheckoutsRepo CheckoutsRepository;
        IPenaltyRepo PenaltyRepository;
        public ReturnController(IReturnRepo returnRepo, IBookRepo bookRepo, IMemberRepo memberRepo, ICheckoutsRepo checkoutsRepo, IPenaltyRepo penaltyRepo)
        {
            ReturnRepository = returnRepo;
            BookRepository = bookRepo;
            MemberRepository = memberRepo;
            CheckoutsRepository = checkoutsRepo;
            PenaltyRepository = penaltyRepo;
        }
        public IActionResult Index()
        {
            List<ReturnViewModel> model = ReturnRepository.GetAllReturns();
            return View(model);
        }
        [HttpGet]
        public IActionResult AddReturn()
        {
            ReturnViewModel ReturnVM = new ReturnViewModel();

            var notAvailableBooks = BookRepository.GetNotAvailableBooks();
            if (notAvailableBooks == null || !notAvailableBooks.Any())
            {
                return View("ErrorInAdding");
            }

            var members = MemberRepository.GetAll();
            if (members == null || !members.Any())
            {
                return View("ErrorInAdding");
            }

            ReturnVM.NotAvailableBooks = notAvailableBooks;
            ReturnVM.Members = members;
            return View(ReturnVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReturn(ReturnViewModel ReturnVM)
        {
            if (ModelState.IsValid)
            {
                Return Return = new Return();
                Book book = BookRepository.GetById(ReturnVM.BookId);
                if (book == null)
                {
                    ModelState.AddModelError("", "Book not found.");
                    return View(ReturnVM);
                }
                book.Availability_Status = "Available";
                BookRepository.Save();

                List<Checkouts> checkoutsList = CheckoutsRepository.GetListByBookId(ReturnVM.BookId);
                if (checkoutsList == null || !checkoutsList.Any())
                {
                    ModelState.AddModelError("", "Checkout records not found.");
                    return View(ReturnVM);
                }
                foreach (var checkouts in checkoutsList)
                {
                    var returnRecord = ReturnRepository.GetByCheckoutsId(checkouts.Id);
                    if (returnRecord == null)
                    {
                        Return.Return_Date = ReturnVM.Return_Date;
                        Return.Checkouts_Id = checkouts.Id;
                        ReturnRepository.Insert(Return);
                        Penalty Pen = new Penalty();
                        Pen.Penalty_Amount = 5;
                        TimeSpan difference = Return.Return_Date - checkouts.Due_Date;
                        if(difference.Days >0)
                            Pen.TotalDelayDays = difference.Days;
                        else
                            Pen.TotalDelayDays = 0;
                        Pen.TotalPenaltyAmount = (float)(Pen.TotalDelayDays * Pen.Penalty_Amount);
                        if (Pen.TotalPenaltyAmount > 0)
                            Pen.Is_Paid = false;
                        else
                            Pen.Is_Paid = true;
                        Pen.Checkouts_Id = Return.Checkouts_Id;
                        PenaltyRepository.Insert(Pen);
                    }
                }
                Penalty penalty = PenaltyRepository.GetByCheckoutsId(Return.Checkouts_Id);
                return RedirectToAction("Details", "Penalty" , penalty);
            }

            return View(ReturnVM);
        }


    }
}
