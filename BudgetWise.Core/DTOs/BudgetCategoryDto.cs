namespace BudgetWise.Core.DTOs
{
    public class BudgetCategoryDto
    {
        public Guid Id { get; set; }
        public Guid BudgetPlanId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    public class CreateBudgetCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    public class UpdateBudgetCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }
}
