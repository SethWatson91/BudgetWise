using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Core.DTOs
{
    public class BudgetCategoryDto
    {
        public Guid Id { get; set; }
        public Guid BudgetPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    public class CreateBudgetCategoryDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "Sort order must be 0 or greater.")]
        public int SortOrder { get; set; }
    }

    public class UpdateBudgetCategoryDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Range(0, int.MaxValue, ErrorMessage = "Sort order must be 0 or greater.")]
        public int SortOrder { get; set; }
    }
}
