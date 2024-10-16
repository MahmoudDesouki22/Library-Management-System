using LibraryFinalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace LibraryFinalProject.ViewModel
{
    public class CheckoutsViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Checkout_Date { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Due_Date { get; set; }
        public double PricePerWeek { get; set; }

        public double TotalPrice { get; set; }
        public int Book_Id { get; set; }
        public int Member_Id { get; set; }
        public string? MemberName { get; set; }
        public int Librarian_Id { get; set; }
        public string? BookTitle { get; set; }
        public List<Book>? books { get; set; }
        public List<Member>? members { get; set; }
        public List<Librarian>? Librarians { get;set; }
    }
}
