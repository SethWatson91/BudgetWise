namespace BudgetWise.Core.Entities
{
    public class BudgetPlan
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        //Navigation
        public ICollection<BudgetCategory> Categories { get; set; } = new List<BudgetCategory>();
    }
}
