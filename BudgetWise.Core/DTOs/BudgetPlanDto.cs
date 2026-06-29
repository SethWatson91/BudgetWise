using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Core.DTOs
{
    public class BudgetPlanDto
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateBudgetPlanDto
    {
        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12.")]
        public int Month { get; set; }
        [Required]
        [Range(2000, 2100, ErrorMessage = "Year must be between 2000 and 2100.")]
        public int Year { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total income must be greater than 0.")]
        public decimal TotalIncome { get; set; }
    }

    public class UpdateBudgetPlanDto
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total income must be greater than 0.")]
        public decimal TotalIncome { get; set; }
    }
}
