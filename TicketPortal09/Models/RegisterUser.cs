using System.ComponentModel.DataAnnotations;

namespace TicketPortal09.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Username is Required")]
        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string? Password { get; set; }


        [Required(ErrorMessage = "ConfirmPassword is Required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }



        [Display(Name = "Role")]
        [Required(ErrorMessage = "Role is Required")]
        public string? Role { get; set; }
      
       
    }
}
