using Asp.Versioning;
using BudgetWise.Core.DTOs;
using BudgetWise.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetWise.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v1/budget-plans/{budgetPlanId}/categories/{budgetCategoryId}/lines/{budgetLineId}/transactions")]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionService;
        public TransactionsController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Returns a paginated list of transactions for a specific budget line.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        /// <param name="budgetLineId">The budget line ID.</param>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Number of results per page (default: 20).</param>
        [HttpGet]
        public async Task<IActionResult> GetAll(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var transactions = await _transactionService.GetAllAsync(budgetLineId, budgetCategoryId, budgetPlanId, GetUserId(), page, pageSize);
            return Ok(transactions);
        }

        /// <summary>
        /// Returns a specific transaction by ID.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        /// <param name="budgetLineId">The budget line ID.</param>
        /// <param name="id">The transaction ID.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, Guid id)
        {
            var transaction = await _transactionService.GetByIdAsync(id, budgetLineId, budgetCategoryId, budgetPlanId, GetUserId());
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        /// <summary>
        /// Records a new transaction against a budget line.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        /// <param name="budgetLineId">The budget line ID.</param>
        [HttpPost]
        public async Task<IActionResult> Create(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, [FromBody] CreateTransactionDto dto)
        {
            var transaction = await _transactionService.CreateAsync(dto, budgetLineId, budgetCategoryId, budgetPlanId, GetUserId());
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        /// <summary>
        /// Updates an existing transaction.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        /// <param name="budgetLineId">The budget line ID.</param>
        /// <param name="id">The transaction ID.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, Guid id, [FromBody] UpdateTransactionDto dto)
        {
            var transaction = await _transactionService.UpdateAsync(id, budgetLineId, budgetCategoryId, budgetPlanId, dto, GetUserId());
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        /// <summary>
        /// Deletes a transaction.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        /// <param name="budgetLineId">The budget line ID.</param>
        /// <param name="id">The transaction ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, Guid id)
        {
            var result = await _transactionService.DeleteAsync(id, budgetLineId, budgetCategoryId, budgetPlanId, GetUserId());
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
