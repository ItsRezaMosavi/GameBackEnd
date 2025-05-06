namespace GameBackEnd.Models.API
{
    public class AuthResponseModel
    {
        public string? UserName {  get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
