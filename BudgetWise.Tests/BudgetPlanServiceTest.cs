using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Infrastructure.Data;
using BudgetWise.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Tests
{
    public class BudgetPlanServiceTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetTaskAsync_ReturnsOnlyUserPlans()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = "user-1";

            context.BudgetPlans.AddRange(
                new BudgetPlan { Id = Guid.NewGuid(), UserId = userId, Month = 1, Year = 2026, TotalIncome = 5000, CreatedAt = DateTimeOffset.UtcNow },
                new BudgetPlan { Id = Guid.NewGuid(), UserId = "user-2", Month = 1, Year = 2024, TotalIncome = 3000, CreatedAt = DateTimeOffset.UtcNow }
            );
            await context.SaveChangesAsync();

            var service = new BudgetPlanService(context);

            // Act
            var result = await service.GetAllAsync(userId);

            // Assert
            result.Should().HaveCount(1);
            result[0].Month.Should().Be(1);
        }

        [Fact]
        public async Task CreateAsync_CreateNewPlan()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = "user-1";
            var service = new BudgetPlanService(context);

            var dto = new CreateBudgetPlanDto
            {
                Month = 6,
                Year = 2026,
                TotalIncome = 5000
            };

            // Act
            var result = await service.CreateAsync(dto, userId);

            // Assert
            result.Should().NotBeNull();
            result.Month.Should().Be(6);
            result.Year.Should().Be(2026);
            result.TotalIncome.Should().Be(5000);
        }

        [Fact]
        public async Task CreateAsync_ThrowException_WhenDuplicatePlan()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = "user-1";

            context.BudgetPlans.Add(
                new BudgetPlan { Id = Guid.NewGuid(), UserId = userId, Month = 6, Year = 2026, TotalIncome = 5000, CreatedAt = DateTimeOffset.UtcNow }
            );
            await context.SaveChangesAsync();

            var service = new BudgetPlanService(context);

            var dto = new CreateBudgetPlanDto
            {
                Month = 6,
                Year = 2026,
                TotalIncome = 4000
            };

            // Act
            var act = async () => await service.CreateAsync(dto, userId);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task DeleteAsync_ReturnFalse_WhenPlanNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new BudgetPlanService(context);
            // Act
            var result = await service.DeleteAsync(Guid.NewGuid(), "user-1");
            // Assert
            result.Should().BeFalse();
        }
    }
}
