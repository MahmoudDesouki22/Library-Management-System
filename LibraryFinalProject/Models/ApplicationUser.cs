using Microsoft.AspNetCore.Identity;

namespace LibraryFinalProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Address { get; set; }
        public string Full_Name { get; set; }



    }
}
