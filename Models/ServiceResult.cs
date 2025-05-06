namespace GameBackEnd.Models
{
    public class ServiceResult
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public string? UserName { get; }
        private ServiceResult(bool isSuccess, string message, string? userName)
        {
            IsSuccess = isSuccess;
            Message = message;
            UserName = userName;
        }

        public static ServiceResult Success(string message) => new(true, message, null);
        public static ServiceResult Success(string message, string username) => new(true, message, username);
        public static ServiceResult Failure(string message) => new(false, message, null);
        
    }
}