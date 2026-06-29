using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Core.Enums;
using BudgetWise.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Infrastructure.Services
{
    public class BudgetLineService
    {
        private readonly AppDbContext _context;

        public BudgetLineService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<BudgetLineDto>> GetAllAsync(Guid budgetCategoryId, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return new List<BudgetLineDto>();

            return await _context.BudgetLines
                .Where(l => l.BudgetCategoryId == budgetCategoryId)
                .OrderBy(l => l.SortOrder)
                .Select(l => new BudgetLineDto
                {
                    Id = l.Id,
                    BudgetCategoryId = l.BudgetCategoryId,
                    Name = l.Name,
                    PlannedAmount = l.PlannedAmount,
                    SpentAmount = l.SpentAmount,
                    Type = l.Type.ToString(),
                    IsRecurring = l.IsRecurring,
                    RecurrenceInterval = l.RecurrenceInterval.ToString(),
                    SortOrder = l.SortOrder
                })
                .ToListAsync();
        }

        public async Task<BudgetLineDto?> GetByIdAsync(Guid id, Guid budgetCategoryId, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return null;

            return await _context.BudgetLines
                .Where(l => l.Id == id && l.BudgetCategoryId == budgetCategoryId)
                .Select(l => new BudgetLineDto
                {
                    Id = l.Id,
                    BudgetCategoryId = l.BudgetCategoryId,
                    Name = l.Name,
                    PlannedAmount = l.PlannedAmount,
                    SpentAmount = l.SpentAmount,
                    Type = l.Type.ToString(),
                    IsRecurring = l.IsRecurring,
                    RecurrenceInterval = l.RecurrenceInterval.ToString(),
                    SortOrder = l.SortOrder
                })
                .FirstOrDefaultAsync();
        }

        public async Task<BudgetLineDto?> CreateAsync(Guid budgetCategoryId, Guid budgetPlanId, CreateBudgetLineDto dto, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return null;

            var categoryExists = await _context.BudgetCategories
                .AnyAsync(c => c.Id == budgetCategoryId && c.BudgetPlanId == budgetPlanId);

            if (!categoryExists)
                return null;

            if (!Enum.TryParse<BudgetLineType>(dto.Type, true, out var lineType))
                throw new ArgumentException($"Invalid budget line type: {dto.Type}");

            RecurrenceInterval? recurrenceInterval = null;
            if (dto.IsRecurring && !string.IsNullOrEmpty(dto.RecurrenceInterval))
            {
                if (!Enum.TryParse<RecurrenceInterval>(dto.RecurrenceInterval, true, out var interval))
                    throw new ArgumentException($"Invalid recurrence interval: {dto.RecurrenceInterval}");
                recurrenceInterval = interval;
            }

            var line = new BudgetLine
            {
                Id = Guid.NewGuid(),
                BudgetCategoryId = budgetCategoryId,
                Name = dto.Name,
                PlannedAmount = dto.PlannedAmount,
                SpentAmount = 0,
                Type = lineType,
                IsRecurring = dto.IsRecurring,
                RecurrenceInterval = recurrenceInterval,
                SortOrder = dto.SortOrder
            };

            _context.BudgetLines.Add(line);
            await _context.SaveChangesAsync();

            return new BudgetLineDto
            {
                Id = line.Id,
                BudgetCategoryId = line.BudgetCategoryId,
                Name = line.Name,
                PlannedAmount = line.PlannedAmount,
                SpentAmount = line.SpentAmount,
                Type = line.Type.ToString(),
                IsRecurring = line.IsRecurring,
                RecurrenceInterval = line.RecurrenceInterval?.ToString(),
                SortOrder = line.SortOrder
            };
        }

        public async Task<BudgetLineDto?> UpdateAsync(Guid id,  Guid budgetCategoryId, Guid budgetPlanId, UpdateBudgetLineDto dto, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return null;

            var line = await _context.BudgetLines
                .FirstOrDefaultAsync(l => l.Id == id && l.BudgetCategoryId == budgetCategoryId);

            if (line == null)
                return null;

            if (!Enum.TryParse<BudgetLineType>(dto.Type, true, out var lineType))
                throw new ArgumentException($"Invalid budget line type: {dto.Type}");

            RecurrenceInterval? recurrenceInterval = null;
            if (dto.IsRecurring && !string.IsNullOrEmpty(dto.RecurrenceInterval))
            {
                if (!Enum.TryParse<RecurrenceInterval>(dto.RecurrenceInterval, true, out var interval))
                    throw new ArgumentException($"Invalid recurrence interval: {dto.RecurrenceInterval}");
                recurrenceInterval = interval;
            }

            line.Name = dto.Name;
            line.PlannedAmount = dto.PlannedAmount;
            line.Type = lineType;
            line.IsRecurring = dto.IsRecurring;
            line.RecurrenceInterval = recurrenceInterval;
            line.SortOrder = dto.SortOrder;

            await _context.SaveChangesAsync();

            return new BudgetLineDto
            {
                Id = line.Id,
                BudgetCategoryId = line.BudgetCategoryId,
                Name = line.Name,
                PlannedAmount = line.PlannedAmount,
                SpentAmount = line.SpentAmount,
                Type = line.Type.ToString(),
                IsRecurring = line.IsRecurring,
                RecurrenceInterval = line.RecurrenceInterval?.ToString(),
                SortOrder = line.SortOrder
            };
        }

        public async Task<bool> DeleteAsync(Guid id, Guid budgetCategoryId, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return false;

            var line = await _context.BudgetLines
                .FirstOrDefaultAsync(l => l.Id == id && l.BudgetCategoryId == budgetCategoryId);

            if (line == null)
                return false;

            _context.BudgetLines.Remove(line);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
