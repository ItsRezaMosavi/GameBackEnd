using System.ComponentModel.DataAnnotations;

namespace GameBackEnd.Models
{
    public class LoginRequest
    {
            [Required(ErrorMessage = "email is required!")]
            [EmailAddress(ErrorMessage = "Email format is not valid!")]
            public string Email { get; set; }

            [Required(ErrorMessage = "password is required!")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
    }
}
