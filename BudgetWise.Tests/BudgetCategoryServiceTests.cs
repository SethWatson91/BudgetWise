using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Infrastructure.Data;
using BudgetWise.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Tests
{
    public class BudgetCategoryServiceTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private BudgetPlan CreatePlan(string userId)
        {
            return new BudgetPlan
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Month = 6,
                Year = 2026,
                TotalIncome = 5000,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenPlanDoesNotBelongToUser()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var plan = CreatePlan("user-1");
            context.BudgetPlans.Add(plan);
            await context.SaveChangesAsync();

            var service = new BudgetCategoryService(context);

            // Act
            var result = await service.GetAllAsync(plan.Id, "user-2");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateAsync_ReturnsNull_WhenPlanDoesNotBelongToUser()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var plan = CreatePlan("user-1");
            context.BudgetPlans.Add(plan);
            await context.SaveChangesAsync();

            var service = new BudgetCategoryService(context);

            var dto = new CreateBudgetCategoryDto { Name = "Housing", SortOrder = 0 };

            // Act
            var result = await service.CreateAsync(plan.Id, dto, "user-2");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_CreatesCategory_WhenPlanBelongsToUser()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = "user-1";
            var plan = CreatePlan(userId);
            context.BudgetPlans.Add(plan);
            await context.SaveChangesAsync();

            var service = new BudgetCategoryService(context);

            var dto = new CreateBudgetCategoryDto { Name = "Housing", SortOrder = 0 };

            // Act
            var result = await service.CreateAsync(plan.Id, dto, userId);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Housing");
            result.BudgetPlanId.Should().Be(plan.Id);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenCategoryNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var plan = CreatePlan("user-1");
            context.BudgetPlans.Add(plan);
            await context.SaveChangesAsync();

            var service = new BudgetCategoryService(context);

            // Act
            var result = await service.DeleteAsync(Guid.NewGuid(), plan.Id, "user-1");

            // Assert
            result.Should().BeFalse();
        }
    }
}
