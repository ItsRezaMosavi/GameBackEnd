using System.ComponentModel.DataAnnotations;

namespace GameBackEnd.Models.API
{
    public class SubmitScoreRequestModel
    {
        [Required(ErrorMessage = "User Name is necessary!")]
        [StringLength(maximumLength: 20, ErrorMessage = "User Name is not  valid", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Score is necessary!")]
        public int Score { get; set; }
    }
}
