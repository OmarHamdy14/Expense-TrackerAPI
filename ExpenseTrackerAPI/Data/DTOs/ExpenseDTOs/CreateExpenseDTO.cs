namespace ExpenseTrackerAPI.Data.DTOs.ExpenseDTOs
{
    public class CreateExpenseDTO
    {
        [Required]
        public double HowMuch { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
    }
}
