using System.ComponentModel.DataAnnotations;

namespace GameBackEnd.Models.API
{
    public class LoginRequestModel
    {
        [Required(ErrorMessage = "email is required!")]
        [EmailAddress(ErrorMessage = "Email format is not valid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
