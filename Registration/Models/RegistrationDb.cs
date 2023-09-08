using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Registration.Models;

public partial class RegistrationDb
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please Enter First Name.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter Last Name.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter Date Of Birth.")]
    public DateTime Dob { get; set; }

    [Required(ErrorMessage = "Please Select Gender.")]
    public string Gender { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter Email.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter Phone No.")]
    public string Phone { get; set; } = null!;

    [Required(ErrorMessage = "Please Enter Username.")]
    public string Username { get; set; } = null!;

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Please Enter Password.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Please Select Department.")]
    public int DepartmentId { get; set; }

    public bool IsActive { get; set; }

    public string? UserImage { get; set; }

    public string? ResetPasswordCode { get; set; }

    [NotMapped]  
    public string DepartmentName { get; set; }

}

