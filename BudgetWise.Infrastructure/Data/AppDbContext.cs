using BudgetWise.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BudgetPlan> BudgetPlans { get; set; }
        public DbSet<BudgetCategory> BudgetCategories { get; set; }
        public DbSet<BudgetLine> BudgetLines { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<SinkingFund> SinkingFunds { get; set; }
        public DbSet<SinkingFundContribution> SinkingFundContributions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            //BudgetPlan
            builder.Entity<BudgetPlan>(entity =>
            {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalIncome).HasPrecision(18, 2);
            entity.HasIndex(e => new { e.UserId, e.Month, e.Year }).IsUnique();
            entity.HasOne(e => e.User)
                .WithMany(e => e.BudgetPlans)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            //BudgetCategory
            builder.Entity<BudgetCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.BudgetPlan)
                      .WithMany(p => p.Categories)
                      .HasForeignKey(e => e.BudgetPlanId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // BudgetLine
            builder.Entity<BudgetLine>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PlannedAmount).HasPrecision(18, 2);
                entity.Property(e => e.SpentAmount).HasPrecision(18, 2);
                entity.HasOne(e => e.BudgetCategory)
                      .WithMany(c => c.BudgetLines)
                      .HasForeignKey(e => e.BudgetCategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Transaction
            builder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.HasIndex(e => e.UserId);
                entity.HasIndex(e => e.TransactionDate);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.BudgetPlan)
                      .WithMany()
                      .HasForeignKey(e => e.BudgetPlanId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.BudgetLine)
                      .WithMany(l => l.Transactions)
                      .HasForeignKey(e => e.BudgetLineId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // SinkingFund
            builder.Entity<SinkingFund>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TargetAmount).HasPrecision(18, 2);
                entity.Property(e => e.CurrentBalance).HasPrecision(18, 2);
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // SinkingFundContribution
            builder.Entity<SinkingFundContribution>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2);
                entity.HasOne(e => e.SinkingFund)
                      .WithMany(f => f.Contributions)
                      .HasForeignKey(e => e.SinkingFundId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.BudgetPlan)
                      .WithMany()
                      .HasForeignKey(e => e.BudgetPlanId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // RefreshToken
            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Token).IsUnique();
                entity.HasIndex(e => e.UserId);
            });
        }
    }
}
