using LibraryFinalProject.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string Availability_Status { get; set; }
    public string ISBN { get; set; }
    public DateTime Publish_Date { get; set; }

    [NotMapped]
    public IFormFile? BookPhotoFile { get; set; } // خاصية رفع الملف

    public string? Book_Photo { get; set; } // مسار الصورة، جعلها اختيارية

    [Display(Name = "Price per Week")]
    public double PricePerWeek { get; set; }

    [ForeignKey("genre")]
    public int Genre_Id { get; set; }
    public Genre? genre { get; set; }
    public ICollection<Checkouts>? checkouts { get; set; }
}
