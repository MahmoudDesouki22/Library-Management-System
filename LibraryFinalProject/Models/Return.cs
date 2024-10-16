using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryFinalProject.Models
{
    public class Return
    {
        public int Id { get; set; }
        public DateTime Return_Date { get; set; }
        [ForeignKey("checkouts")]
        public int Checkouts_Id { get; set; }
        public Checkouts? checkouts { get; set; }
    }
}
