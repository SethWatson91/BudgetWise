using Azure;
using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Core.Enums;
using BudgetWise.Core.Models;
using BudgetWise.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Infrastructure.Services
{
    public class TransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TransactionDto>> GetAllAsync(Guid budgetLineId, Guid budgetCategoryId, Guid budgetPlanId, string userId, int page = 1, int pageSize = 20)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);
            
            if (!planExists)
                return new PagedResult<TransactionDto>();

            var query = _context.Transactions
                .Where(t => t.BudgetLineId == budgetLineId)
                .OrderByDescending(t => t.TransactionDate);

            var totalCount = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    BudgetLineId = t.BudgetLineId,
                    BudgetPlanId = t.BudgetPlanId,
                    Amount = t.Amount,
                    Description = t.Description,
                    TransactionDate = t.TransactionDate,
                    Type = t.Type.ToString(),
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<TransactionDto>
            {
                Data = data,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            };

        }

        public async Task<TransactionDto?> GetByIdAsync(Guid id, Guid budgetLineId, Guid budgetCategoryId, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);
            
            if (!planExists)
                return null;
            
            return await _context.Transactions
                .Where(t => t.Id == id && t.BudgetLineId == budgetLineId)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    BudgetLineId = t.BudgetLineId,
                    BudgetPlanId = t.BudgetPlanId,
                    Amount = t.Amount,
                    Description = t.Description,
                    TransactionDate = t.TransactionDate,
                    Type = t.Type.ToString(),
                    CreatedAt = t.CreatedAt

                })
                .FirstOrDefaultAsync();
        }

        public async Task<TransactionDto?> CreateAsync(CreateTransactionDto dto, Guid budgetLineId, Guid budgetCategoryId, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);
            
            if (!planExists)
                return null;

            var lineExists = await _context.BudgetLines
                .AnyAsync(l => l.Id == budgetLineId && l.BudgetCategoryId == budgetCategoryId);

            if (!lineExists)
                return null;

            if (!Enum.TryParse<TransactionType>(dto.Type, true, out var transactionType))
                throw new ArgumentException($"Invalid transaction type: {dto.Type}");

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                BudgetLineId = budgetLineId,
                BudgetPlanId = budgetPlanId,
                Amount = dto.Amount,
                Description = dto.Description,
                TransactionDate = dto.TransactionDate,
                Type = transactionType,
                CreatedAt = DateTimeOffset.UtcNow,
                UserId = userId
            };
            
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            
            return new TransactionDto
            {
                Id = transaction.Id,
                BudgetLineId = transaction.BudgetLineId,
                BudgetPlanId = transaction.BudgetPlanId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                Type = transaction.Type.ToString(),
                CreatedAt = transaction.CreatedAt
            };
        }

        public async Task<TransactionDto?> UpdateAsync( Guid id, Guid budgetLineId, Guid budgetCategoryId, Guid budgetPlanId, UpdateTransactionDto transactionDto, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);

            if (!planExists)
                return null;

            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.BudgetLineId == budgetLineId);

            if (transaction == null)
                return null;

            if (!Enum.TryParse<TransactionType>(transactionDto.Type, true, out var transactionType))
                throw new ArgumentException($"Invalid transaction type: {transactionDto.Type}");

            transaction.Type = transactionType;
            transaction.Amount = transactionDto.Amount;
            transaction.Description = transactionDto.Description;
            transaction.TransactionDate = transactionDto.TransactionDate;

            await _context.SaveChangesAsync();

            return new TransactionDto
            {
                Id = transaction.Id,
                BudgetLineId = transaction.BudgetLineId,
                BudgetPlanId = transaction.BudgetPlanId,
                Amount = transaction.Amount,
                Description = transaction.Description,
                TransactionDate = transaction.TransactionDate,
                Type = transaction.Type.ToString(),
                CreatedAt = transaction.CreatedAt

            };
        }

        public async Task<bool> DeleteAsync(Guid id, Guid budgetLineId, Guid budgetCategoryId, Guid budgetPlanId, string userId)
        {
            var planExists = await _context.BudgetPlans
                .AnyAsync(p => p.Id == budgetPlanId && p.UserId == userId);
            
            if (!planExists)
                return false;
            
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.Id == id && t.BudgetLineId == budgetLineId);
            
            if (transaction == null)
                return false;
            
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
