using LibraryFinalProject.IRepository;
using LibraryFinalProject.Models;
using LibraryFinalProject.Models.DbContext;

namespace LibraryFinalProject.Repository
{
    public class GenreRepo : IGenreRepo
    {
        ApplicationDbContext Context;
        public GenreRepo(ApplicationDbContext db)
        {
            Context = db;
        }
        public List<Genre> GetAll()
        {
            return Context.Genres.ToList();
        }

        public Genre GetById(int id)
        {
            return Context.Genres.FirstOrDefault(G => G.Id == id);
        }

        public void Insert(Genre genre)
        {
            Context.Genres.Add(genre);
            Context.SaveChanges();
        }

        public void Update(Genre genre, int id)
        {
            Genre OldGenre = GetById(id);
            if (OldGenre != null)
            {
                OldGenre.Name = genre.Name;
                Context.SaveChanges();
            }
        }
    }
}
