using LibraryFinalProject.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LibraryFinalProject.Controllers
{
    public class MemberController : Controller
    {
        IMemberRepo memberRepo;
        public MemberController(IMemberRepo memberRepo)
        {
            this.memberRepo = memberRepo;
        }
        public IActionResult Details(int id)
        {
            return View();
        }

        [Authorize]
        public IActionResult MyHistory()
        {
          
                string username = User.Identity.Name;
               

             var result =   memberRepo.historyBookViewModels(username);
                return View(result);
 

           
        }
    }
}
