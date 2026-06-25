namespace BudgetWise.Core.Entities
{
    public class BudgetCategory
    {
        public Guid Id { get; set; }
        public Guid BudgetPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        //Navigation
        public BudgetPlan BudgetPlan { get; set; } = null!;
        public ICollection<BudgetLine> BudgetLines { get; set; } = new List<BudgetLine>();
    }
}
