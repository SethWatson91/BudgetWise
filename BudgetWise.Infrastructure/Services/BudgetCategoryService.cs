using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BudgetWise.Infrastructure.Services
{
    public class BudgetCategoryService
    {
        private readonly AppDbContext _context;
        public BudgetCategoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<BudgetCategoryDto>> GetAllAsync(Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return new List<BudgetCategoryDto>();

            return await _context.BudgetCategories
                .Where(c => c.BudgetPlanId == budgetPlanId)
                .OrderBy(c => c.SortOrder)
                .Select(c => new BudgetCategoryDto
                {
                    Id = c.Id,
                    BudgetPlanId = c.BudgetPlanId,
                    Name = c.Name,
                    SortOrder = c.SortOrder
                })
                .ToListAsync();
        }

        public async Task<BudgetCategoryDto?> GetByIdAsync(Guid id, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return null;

            return await _context.BudgetCategories
                .Where(c => c.Id == id && c.BudgetPlanId == budgetPlanId)
                .Select(c => new BudgetCategoryDto
                {
                    Id = c.Id,
                    BudgetPlanId = c.BudgetPlanId,
                    Name = c.Name,
                    SortOrder = c.SortOrder
                })
                .FirstOrDefaultAsync();
        }

        public async Task<BudgetCategoryDto?> CreateAsync(Guid budgetPlanId, CreateBudgetCategoryDto dto, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return null;

            var category = new BudgetCategory
            {
                Id = Guid.NewGuid(),
                BudgetPlanId = budgetPlanId,
                Name = dto.Name,
                SortOrder = dto.SortOrder
            };

            _context.BudgetCategories.Add(category);
            await _context.SaveChangesAsync();

            return new BudgetCategoryDto
            {
                Id = category.Id,
                BudgetPlanId = category.BudgetPlanId,
                Name = category.Name,
                SortOrder = category.SortOrder
            };
        }

        public async Task<BudgetCategoryDto?> UpdateAsync(Guid id, Guid budgetPlanId, UpdateBudgetCategoryDto dto, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return null;

            var category = await _context.BudgetCategories
                .FirstOrDefaultAsync(c => c.Id == id && c.BudgetPlanId == budgetPlanId);

            if (category == null)
                return null;

            category.Name = dto.Name;
            category.SortOrder = dto.SortOrder;
            await _context.SaveChangesAsync();

            return new BudgetCategoryDto
            {
                Id = category.Id,
                BudgetPlanId = category.BudgetPlanId,
                Name = category.Name,
                SortOrder = category.SortOrder
            };
        }

        public async Task<bool> DeleteAsync(Guid id, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return false;

            var category = await _context.BudgetCategories
                .FirstOrDefaultAsync(c => c.Id == id && c.BudgetPlanId == budgetPlanId);

            if (category == null)
                return false;

            _context.BudgetCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
