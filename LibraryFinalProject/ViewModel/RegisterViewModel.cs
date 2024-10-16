using System.ComponentModel.DataAnnotations;

namespace LibraryFinalProject.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User Name is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Full Name is Required")]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter Right Email Address")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber , ErrorMessage ="Enter Right Phone Number")]
        [Display(Name ="Phone Number")]
        public string Phone { get; set; }
    }
}
