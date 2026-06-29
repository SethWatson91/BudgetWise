using BudgetWise.Core.DTOs;
using BudgetWise.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetWise.API.Controllers
{
    [ApiController]
    [Route("api/v1/budget-plans/{budgetPlanId}/categories/{budgetCategoryId}/lines")]
    [Authorize]
    public class BudgetLinesController : ControllerBase
    {
        private readonly BudgetLineService _budgetLineService;
        public BudgetLinesController(BudgetLineService budgetLineService)
        {
            _budgetLineService = budgetLineService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Returns all budget lines for a specific category.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        [HttpGet]
        public async Task<IActionResult> GetAll( Guid budgetCategoryId, Guid budgetPlanId)
        {
            var budgetLines = await _budgetLineService.GetAllAsync(budgetCategoryId, budgetPlanId, GetUserId());
            return Ok(budgetLines);
        }

        /// <summary>
        /// Returns a specific budget line by ID.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        /// <param name="id">The budget line ID.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById( Guid budgetCategoryId, Guid budgetPlanId, Guid id)
        {
            var budgetLine = await _budgetLineService.GetByIdAsync(id, budgetCategoryId, budgetPlanId, GetUserId());
            if (budgetLine == null)
                return NotFound();
            return Ok(budgetLine);
        }

        /// <summary>
        /// Creates a new budget line within a category.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        [HttpPost]
        public async Task<IActionResult> Create(Guid budgetCategoryId, Guid budgetPlanId, [FromBody] CreateBudgetLineDto dto)
        {
            var budgetLine = await _budgetLineService.CreateAsync(budgetCategoryId, budgetPlanId, dto, GetUserId());
            if (budgetLine == null)
                return NotFound();
            return Ok(budgetLine);
        }

        /// <summary>
        /// Updates an existing budget line.
        /// </summary>
        /// <param name="id">The budget line ID.</param>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Guid budgetCategoryId, Guid budgetPlanId, [FromBody] UpdateBudgetLineDto dto)
        {
            var budgetLine = await _budgetLineService.UpdateAsync(id, budgetCategoryId, budgetPlanId, dto, GetUserId());
            if (budgetLine == null)
                return NotFound();
            return Ok(budgetLine);
        }

        /// <summary>
        /// Deletes a budget line and all associated transactions.
        /// </summary>
        /// <param name="id">The budget line ID.</param>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="budgetCategoryId">The category ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, Guid budgetCategoryId, Guid budgetPlanId)
        {
            var success = await _budgetLineService.DeleteAsync(id, budgetCategoryId, budgetPlanId, GetUserId());
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
