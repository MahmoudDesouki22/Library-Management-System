using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryFinalProject.Models
{
    public class Checkouts
    {
        public int Id { get; set; }
        public DateTime Checkout_Date { get; set; }
        public DateTime Due_Date { get; set; }
        [ForeignKey("book")]
        public int Book_Id { get; set; }
        public Book? book { get; set; }
        [ForeignKey("member")]
        public int Member_Id { get; set; }
        public Member? member { get; set; }
        [ForeignKey("librarian")]
        public int Librarian_Id { get; set; }
        public Librarian? librarian { get; set; }
    }
}
