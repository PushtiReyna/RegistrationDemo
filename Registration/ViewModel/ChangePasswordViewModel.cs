using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.ViewModel
{
    public class ChangePasswordViewModel
    {

        public int? Id { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; } = null!;


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New password required")]
        [MinLength(5, ErrorMessage = "The password must be at least 5 characters")]
        [MaxLength(8, ErrorMessage = "The password cannot be more than 8 characters")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must contain uppercase letter,lowercase letter and special chararcters.")]
        public string NewPassword { get; set; }

       
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }
    }
}
