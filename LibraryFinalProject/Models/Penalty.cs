using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryFinalProject.Models
{
    public class Penalty
    {
        public int Id { get; set; }
        public Double Penalty_Amount { get; set; }
        public int TotalDelayDays { get; set; }
        public float TotalPenaltyAmount { get; set; }
        public bool Is_Paid { get; set; }
        [ForeignKey("checkouts")]
        public int Checkouts_Id { get; set; }
        public Checkouts? checkouts { get; set; }
    }
}
