namespace ExpenseTrackerAPI.Data.DTOs.ExpenseCategoryDTOs
{
    public class UpdateExpenseCategoryDTO
    {
        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
