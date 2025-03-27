namespace ExpenseTrackerAPI.Data.DTOs.AccountDTOs
{
    public class LogInDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
