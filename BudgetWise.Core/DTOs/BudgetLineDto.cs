using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Core.DTOs
{
    public class BudgetLineDto
    {
        public Guid Id { get; set; }
        public Guid BudgetCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PlannedAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        public int SortOrder { get; set; }
    }

    public class CreateBudgetLineDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Planned amount must be greater than 0.")]
        public decimal PlannedAmount { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Sort order must be 0 or greater.")]
        public int SortOrder { get; set; }
    }

    public class UpdateBudgetLineDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Planned amount must be greater than 0.")]
        public decimal PlannedAmount { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Sort order must be 0 or greater.")]
        public int SortOrder { get; set; }
    }
}
