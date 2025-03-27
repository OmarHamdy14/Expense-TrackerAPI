namespace ExpenseTrackerAPI.Models
{
    public class AuthModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
