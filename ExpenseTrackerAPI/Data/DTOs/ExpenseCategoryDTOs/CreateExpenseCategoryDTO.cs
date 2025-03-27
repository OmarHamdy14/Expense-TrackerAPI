namespace ExpenseTrackerAPI.Data.DTOs.ExpenseCategoryDTOs
{
    public class CreateExpenseCategoryDTO
    {
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string UserId { get; set; }
    }
}
