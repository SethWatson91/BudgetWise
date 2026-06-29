namespace BudgetWise.Core.DTOs
{
    public class BudgetLineDto
    {
        public Guid Id { get; set; }
        public Guid BudgetCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PlannedAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        public int SortOrder { get; set; }
    }

    public class CreateBudgetLineDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal PlannedAmount { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        public int SortOrder { get; set; }
    }

    public class UpdateBudgetLineDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal PlannedAmount { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsRecurring { get; set; }
        public string? RecurrenceInterval { get; set; }
        public int SortOrder { get; set; }
    }
}
