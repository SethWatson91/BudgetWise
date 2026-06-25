namespace BudgetWise.Core.Entities
{
    public class SinkingFund
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal TargetAmount { get; set; }
        public DateOnly? TargetDate { get; set; }
        public decimal CurrentBalance { get; set; }
        public bool IsActive { get; set; }

        //Navigation
        public User User { get; set; } = null!;
        public ICollection<SinkingFundContribution> Contributions { get; set; } = new List<SinkingFundContribution>();

    }
}
