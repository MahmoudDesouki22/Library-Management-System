using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryFinalProject.Controllers
{
    public class GenreController : Controller
    {
        IGenreRepo GenreRepository;
        public GenreController(IGenreRepo genreRepo)
        {
            GenreRepository = genreRepo;
        }
        public IActionResult Index()
        {
            List<Genre> genreList = GenreRepository.GetAll();
            return View(genreList);
        }
        [HttpGet]
        public IActionResult AddGenre()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                GenreRepository.Insert(genre);
                return RedirectToAction("Index");
            }
            return View(genre);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Genre genre = GenreRepository.GetById(id);
            return View(genre);
        }
        [HttpPost]
        public IActionResult Edit(int id , Genre genre)
        {
            if(ModelState.IsValid)
            {
                GenreRepository.Update(genre, id);
                return RedirectToAction("Index");
            }
            return View(genre);
        }
    }
}
