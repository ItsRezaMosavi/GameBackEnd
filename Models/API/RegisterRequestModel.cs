using System.ComponentModel.DataAnnotations;

namespace GameBackEnd.Models.API
{
    public class RegisterRequestModel
    {

        [Required(ErrorMessage = "Email is necessary!")]
        [EmailAddress(ErrorMessage = "Email format is not correct!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Name is necessary!")]
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "User Name length must be between 3 to 20 character!")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is necessary")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "password length must be at least 8 character")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "password must contain capital and non-capital words and numbers!")]
        public string Password { get; set; }

    }
}
