namespace ExpenseTrackerAPI.Data.DTOs.AccountDTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}