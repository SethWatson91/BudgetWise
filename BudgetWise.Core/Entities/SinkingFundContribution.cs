namespace BudgetWise.Core.Entities
{
    public class SinkingFundContribution
    {
        public Guid Id { get; set; }
        public Guid SinkingFundId { get; set; }
        public Guid BudgetPlanId { get; set; }
        public decimal Amount { get; set; }
        public DateOnly ContributionDate { get; set; }

        //Navigation
        public SinkingFund SinkingFund { get; set; } = null!;
        public BudgetPlan BudgetPlan { get; set; } = null!;
    }
}
