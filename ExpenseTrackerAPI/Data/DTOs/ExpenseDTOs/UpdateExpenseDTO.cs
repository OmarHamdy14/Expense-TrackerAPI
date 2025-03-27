namespace ExpenseTrackerAPI.Data.DTOs.ExpenseDTOs
{
    public class UpdateExpenseDTO
    {
        [Required]
        public double HowMuch { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}
