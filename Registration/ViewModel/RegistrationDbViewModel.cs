using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.ViewModel
{
    public class RegistrationDbViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter First Name")]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Please enter only letters for First Name.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter Last Name")]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Please enter only letters for Last Name.")]
        public string LastName { get; set; } = null!;

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Please enter Date Of Birth")]
        public DateTime Dob { get; set; }

        
        [Required(ErrorMessage = "Please Select Gender.")]
        public string Gender { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter Email.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter Phone No.")]
        [MinLength(10, ErrorMessage = "The Phone No. must be at least 10 characters")]
        [MaxLength(10, ErrorMessage = "The Phone No. cannot be more than 10 characters")]

        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter Username."), MaxLength(10)]
        [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9 ]+[a-zA-Z0-9]$", ErrorMessage = "Username must contain uppercase letter, lowercase letter and numbers.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Please Enter Password.")]
        [MinLength(5, ErrorMessage = "The password must be at least 5 characters")]
        [MaxLength(8, ErrorMessage = "The password cannot be more than 8 characters")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must contain uppercase letter,lowercase letter and special chararcters.")]

        public string Password { get; set; } = null!;

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please Select Department.")]
        public int DepartmentId { get; set; }

        public bool IsActive { get; set; }
        public IFormFile? UserImage { get; set; }

        [NotMapped]
        public string DepartmentName { get; set; }
       
    }
}
