using System.ComponentModel.DataAnnotations;

namespace GameBackEnd.Models.API
{
    public class AuthResponseModel
    {
        [Required]
        public string UserName {  get; set; }
        [Required]
        public string AccessToken {  get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int ExpiresIn {  get; set; }
    }
}
