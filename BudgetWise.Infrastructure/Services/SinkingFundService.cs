using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Infrastructure.Services
{
    public class SinkingFundService
    {
        private readonly AppDbContext _context;

        public SinkingFundService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SinkingFundDto>> GetAllAsync(string userId)
        {
            return await _context.SinkingFunds
                .Where(f => f.UserId == userId)
                .Select(f => new SinkingFundDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    TargetAmount = f.TargetAmount,
                    TargetDate = f.TargetDate,
                    CurrentBalance = f.CurrentBalance,
                    IsActive = f.IsActive
                })
                .ToListAsync();
        }

        public async Task<SinkingFundDto?> GetByIdAsync(Guid id, string userId)
        {
            return await _context.SinkingFunds
                .Where(f => f.Id == id && f.UserId == userId)
                .Select(f => new SinkingFundDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    TargetAmount = f.TargetAmount,
                    TargetDate = f.TargetDate,
                    CurrentBalance = f.CurrentBalance,
                    IsActive = f.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<SinkingFundDto> CreateAsync(CreateSinkingFundDto dto, string userId)
        {
            var sinkingFund = new SinkingFund
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                TargetAmount = dto.TargetAmount,
                TargetDate = dto.TargetDate,
                CurrentBalance = dto.CurrentBalance,
                IsActive = dto.IsActive,
                UserId = userId
            };

            _context.SinkingFunds.Add(sinkingFund);
            await _context.SaveChangesAsync();
            return new SinkingFundDto
            {
                Id = sinkingFund.Id,
                Name = sinkingFund.Name,
                TargetAmount = sinkingFund.TargetAmount,
                TargetDate = sinkingFund.TargetDate,
                CurrentBalance = sinkingFund.CurrentBalance,
                IsActive = sinkingFund.IsActive
            };
        }

        public async Task<SinkingFundDto?> UpdateAsync(Guid id, UpdateSinkingFundDto dto, string userId)
        {
            var sinkingFund = await _context.SinkingFunds
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
            if (sinkingFund == null)
                return null;
            sinkingFund.Name = dto.Name;
            sinkingFund.TargetAmount = dto.TargetAmount;
            sinkingFund.TargetDate = dto.TargetDate;
            sinkingFund.CurrentBalance = dto.CurrentBalance;
            sinkingFund.IsActive = dto.IsActive;
            await _context.SaveChangesAsync();
            return new SinkingFundDto
            {
                Id = sinkingFund.Id,
                Name = sinkingFund.Name,
                TargetAmount = sinkingFund.TargetAmount,
                TargetDate = sinkingFund.TargetDate,
                CurrentBalance = sinkingFund.CurrentBalance,
                IsActive = sinkingFund.IsActive
            };
        }

        public async Task<bool> DeleteAsync(Guid id, string userId)
        {
            var sinkingFund = await _context.SinkingFunds
                .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
            if (sinkingFund == null)
                return false;
            _context.SinkingFunds.Remove(sinkingFund);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}