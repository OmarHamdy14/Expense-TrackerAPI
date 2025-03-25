using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerAPI.Models
{
    public class ExpenseCategory
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
