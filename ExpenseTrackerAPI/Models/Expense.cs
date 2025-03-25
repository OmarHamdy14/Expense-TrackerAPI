using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Models
{
    public class Expense
    {
        [Required]
        public double HowMuch { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        [Required]
        public ExpenseCategory Category { get; set; }
    }
}
