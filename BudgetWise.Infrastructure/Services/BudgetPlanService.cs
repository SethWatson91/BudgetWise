using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BudgetWise.Infrastructure.Services
{
    public class BudgetPlanService
    {
        private readonly AppDbContext _context;

        public BudgetPlanService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BudgetPlanDto>> GetAllAsync(string userId)
        {
            return await _context.BudgetPlans
                .Where(p => p.UserId == userId)
                .Select(p => new BudgetPlanDto
                {
                    Id = p.Id,
                    Month = p.Month,
                    Year = p.Year,
                    TotalIncome = p.TotalIncome,
                    CreatedAt = p.CreatedAt
                })
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();
        }

        public async Task<BudgetPlanDto?> GetByIdAsync(Guid id, string userId)
        {
            return await _context.BudgetPlans
                .Where(p => p.Id == id && p.UserId == userId)
                .Select(p => new BudgetPlanDto
                {
                    Id = p.Id,
                    Month = p.Month,
                    Year = p.Year,
                    TotalIncome = p.TotalIncome,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();
        }

        public async Task<BudgetPlanDto> CreateAsync(CreateBudgetPlanDto dto, string userId)
        {
            var exists = await _context.BudgetPlans
                .AnyAsync(p => p.UserId == userId && p.Month == dto.Month && p.Year == dto.Year);

            if (exists)
                throw new InvalidOperationException("A budget plan already exists for this month and year.");

            var plan = new BudgetPlan
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Month = dto.Month,
                Year = dto.Year,
                TotalIncome = dto.TotalIncome,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _context.BudgetPlans.Add(plan);
            await _context.SaveChangesAsync();

            return new BudgetPlanDto
            {
                Id = plan.Id,
                Month = plan.Month,
                Year = plan.Year,
                TotalIncome = plan.TotalIncome,
                CreatedAt = plan.CreatedAt
            };
        }

        public async Task<BudgetPlanDto?> UpdateAsync(Guid id, UpdateBudgetPlanDto dto, string userId)
        {
            var plan = await _context.BudgetPlans
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plan == null)
                return null;

            plan.TotalIncome = dto.TotalIncome;
            await _context.SaveChangesAsync();

            return new BudgetPlanDto
            {
                Id = plan.Id,
                Month = plan.Month,
                Year = plan.Year,
                TotalIncome = plan.TotalIncome,
                CreatedAt = plan.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id, string userId)
        {
            var plan = await _context.BudgetPlans
                .FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

            if (plan == null)
                return false;

            _context.BudgetPlans.Remove(plan);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
