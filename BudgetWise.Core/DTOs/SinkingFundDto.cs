using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Core.DTOs
{
    public class SinkingFundDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public DateOnly? TargetDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateSinkingFundDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than 0.")]
        public decimal TargetAmount { get; set; }
        public DateOnly? TargetDate { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Current balance cannot be negative.")]
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateSinkingFundDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Target amount must be greater than 0.")]
        public decimal TargetAmount { get; set; }
        public DateOnly? TargetDate { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Current balance cannot be negative.")]
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
    }
}
