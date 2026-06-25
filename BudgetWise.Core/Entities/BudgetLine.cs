namespace BudgetWise.Core.Entities
{
    using BudgetWise.Core.Enums;

    public class BudgetLine
    {
        public Guid Id { get; set; }
        public Guid BudgetCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PlannedAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public BudgetLineType Type { get; set; }
        public bool IsRecurring { get; set; }
        public RecurrenceInterval? RecurrenceInterval { get; set; }
        public int SortOrder { get; set; }

        //Navigation
        public BudgetCategory BudgetCategory { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
