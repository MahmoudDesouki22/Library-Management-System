using LibraryFinalProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryFinalProject.ViewModel
{
    public class PenaltyViewMode
    {

        public int Id { get; set; }
        public int TotalDelayDays { get; set; }
        public float TotalPenaltyAmount { get; set; }
        public bool Is_Paid { get; set; }
        public int Checkouts_Id { get; set; }
        public string? FullName { get; set; }
        public DateTime Due_Date { get; set; }
        public string? Title { get; set; }

        public DateTime? Return_Date { get; set; }



    }
}
