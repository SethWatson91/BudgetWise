using BudgetWise.Core.DTOs;
using BudgetWise.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetWise.API.Controllers
{
    [ApiController]
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


        [HttpGet]
        public async Task<IActionResult> GetAll(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var transactions = await _transactionService.GetAllAsync(budgetLineId, budgetCategoryId, budgetPlanId, GetUserId(), page, pageSize);
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, Guid id)
        {
            var transaction = await _transactionService.GetByIdAsync(id, budgetLineId, budgetCategoryId, budgetPlanId, GetUserId());
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, [FromBody] CreateTransactionDto dto)
        {
            var transaction = await _transactionService.CreateAsync(dto, budgetLineId, budgetCategoryId, budgetPlanId, GetUserId());
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid budgetPlanId, Guid budgetCategoryId, Guid budgetLineId, Guid id, [FromBody] UpdateTransactionDto dto)
        {
            var transaction = await _transactionService.UpdateAsync(id, budgetLineId, budgetCategoryId, budgetPlanId, dto, GetUserId());
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

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
