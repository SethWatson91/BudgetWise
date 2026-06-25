using Microsoft.AspNetCore.Identity;

namespace BudgetWise.Infrastructure.Data
{
    public class AppUser : IdentityUser
    {
        public DateTimeOffset CreatedAt { get; set; }
    }
}
