namespace BudgetWise.Core.Entities
{
    public class User
    {
        public string Id {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }

        //Navigation
        public ICollection<BudgetPlan> BudgetPlans { get; set; } = new List<BudgetPlan>();
    }
}
