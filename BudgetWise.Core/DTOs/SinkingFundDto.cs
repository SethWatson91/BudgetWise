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
        public string Name { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public DateOnly? TargetDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
    }
    public class UpdateSinkingFundDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public DateOnly? TargetDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }
    }
}
