namespace BudgetWise.Core.Entities
{
    using BudgetWise.Core.Enums;

    public class Transaction
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Guid? BudgetLineId { get; set; }
        public Guid BudgetPlanId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly TransactionDate {  get; set; }
        public TransactionType Type { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        //Navigation
        public User User { get; set; } = null!;
        public BudgetLine? BudgetLine { get; set; }
        public BudgetPlan BudgetPlan { get; set; } = null!;
    }
}
