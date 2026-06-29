using BudgetWise.Core.DTOs;
using BudgetWise.Core.Entities;
using BudgetWise.Infrastructure.Data;
using BudgetWise.Infrastructure.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Tests
{
    public class SinkingFundServiceTests
    {
        private AppDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOnlyUserFunds()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = "user-1";

            context.SinkingFunds.AddRange(
                new SinkingFund { Id = Guid.NewGuid(), UserId = userId, Name = "Christmas", TargetAmount = 1000, CurrentBalance = 0, IsActive = true },
                new SinkingFund { Id = Guid.NewGuid(), UserId = "user-2", Name = "Vacation", TargetAmount = 2000, CurrentBalance = 0, IsActive = true }
            );
            await context.SaveChangesAsync();

            var service = new SinkingFundService(context);

            // Act
            var result = await service.GetAllAsync(userId);

            // Assert
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("Christmas");
        }

        [Fact]
        public async Task CreateAsync_CreatesNewFund()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var userId = "user-1";
            var service = new SinkingFundService(context);

            var dto = new CreateSinkingFundDto
            {
                Name = "Car Repair",
                TargetAmount = 500,
                CurrentBalance = 0,
                IsActive = true
            };

            // Act
            var result = await service.CreateAsync(dto, userId);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Car Repair");
            result.TargetAmount.Should().Be(500);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenFundNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new SinkingFundService(context);

            // Act
            var result = await service.DeleteAsync(Guid.NewGuid(), "user-1");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenFundNotFound()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var service = new SinkingFundService(context);

            var dto = new UpdateSinkingFundDto
            {
                Name = "Updated",
                TargetAmount = 1000,
                CurrentBalance = 0,
                IsActive = true
            };

            // Act
            var result = await service.UpdateAsync(Guid.NewGuid(), dto, "user-1");

            // Assert
            result.Should().BeNull();
        }
    }
}