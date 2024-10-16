using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryFinalProject.Controllers
{
    [Authorize]
    public class PenaltyController : Controller
    {
        IPenaltyRepo PenaltyRepository;
        ICheckoutsRepo CheckoutsRepository;
        IBookRepo BookRepository;
        IMemberRepo MemberRepository;
        IReturnRepo ReturnRepository;
        public PenaltyController(IPenaltyRepo PenaltyRepo, ICheckoutsRepo checkoutsRepo, IBookRepo bookRepo, IMemberRepo memberRepo, IReturnRepo returnRepo)
        {
            PenaltyRepository = PenaltyRepo;
            CheckoutsRepository = checkoutsRepo;
            BookRepository = bookRepo;
            MemberRepository = memberRepo;
            ReturnRepository = returnRepo;
        }
        [Authorize(Roles = "Librarian")]
        public IActionResult Index()
        {
            return View(PenaltyRepository.GetAll());
        }
        [Authorize(Roles = "Librarian")]

        public IActionResult Details(Penalty penalty)
        {
            PenaltyViewMode penaltyView = new PenaltyViewMode();
            penaltyView = PenaltyRepository.GetAllInformation(penalty.Id);
            if (penaltyView.TotalPenaltyAmount == 0)
            {
                penaltyView.Is_Paid = true;
            }
            return View(penaltyView);
        }
        [HttpPost]
        [Authorize(Roles = "Librarian")]

        public IActionResult Pay(int id)
        {
            Penalty penalty = PenaltyRepository.GetById(id);
            penalty.Is_Paid = true;
            PenaltyRepository.save();
            return RedirectToAction("Details", new { Id = penalty.Id });
        }
        public IActionResult IndexForMembers(string user)
        {
            List<PenaltyViewMode> penaltyViewModes = PenaltyRepository.GetAllInformationByUserName(user);
            return View(penaltyViewModes);
        }
    }
}
