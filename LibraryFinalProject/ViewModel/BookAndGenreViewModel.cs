using LibraryFinalProject.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryFinalProject.ViewModel
{
    public class BookAndGenreViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Availability_Status { get; set; }
        public string ISBN { get; set; }
        public DateTime Publish_Date { get; set; }

        [Display(Name = "Book Photo")]
        public IFormFile? BookPhotoFile { get; set; }

        public string? Book_Photo { get; set; }

        [Display(Name = "Price per Week")]
        public double PricePerWeek { get; set; }

        public int Genre_Id { get; set; }
        public IEnumerable<Genre>? genres { get; set; }
        public string? Genre_Name { get; set; }
        public string? Availability_color { get; set; }
    }
}
