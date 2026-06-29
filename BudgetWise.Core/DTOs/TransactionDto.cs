using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Core.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid? BudgetLineId { get; set; }
        public Guid BudgetPlanId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly TransactionDate { get; set; }
        public string Type { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateTransactionDto
    {
        public Guid? BudgetLineId { get; set; }
        public Guid BudgetPlanId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateOnly TransactionDate { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
    }

    public class UpdateTransactionDto
    {
        public Guid? BudgetLineId { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters.")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateOnly TransactionDate { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
    }
}
