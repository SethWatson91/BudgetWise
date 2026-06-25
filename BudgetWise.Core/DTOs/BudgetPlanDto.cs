namespace BudgetWise.Core.DTOs
{
    public class BudgetPlanDto
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public class CreateBudgetPlanDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal TotalIncome { get; set; }
    }

    public class UpdateBudgetPlanDto
    {
        public decimal TotalIncome { get; set; }
    }
}
