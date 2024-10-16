using LibraryFinalProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryFinalProject.ViewModel
{
    public class ReturnViewModel
    {
        public int Id { get; set; }
        public DateTime Return_Date { get; set; }
        public int? Checkouts_Id { get; set; }
        public string? BookTitle { get; set; }
        public string? MemberName { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public List<Member>? Members { get; set; }
        public List<Book>? NotAvailableBooks { get; set; }

    }
}
