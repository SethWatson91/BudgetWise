using BudgetWise.Core.DTOs;
using BudgetWise.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetWise.API.Controllers
{
    [ApiController]
    [Route("api/v1/budget-plans/{budgetPlanId}/categories")]
    [Authorize]
    public class BudgetCategoriesController : ControllerBase
    {
        private readonly BudgetCategoryService _budgetCategoryService;

        public BudgetCategoriesController(BudgetCategoryService budgetCategoryService)
        {
            _budgetCategoryService = budgetCategoryService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Returns all categories for a specific budget plan.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        [HttpGet]
        public async Task<IActionResult> GetAll(Guid budgetPlanId)
        {
            var categories = await _budgetCategoryService.GetAllAsync(budgetPlanId, GetUserId());
            return Ok(categories);
        }

        /// <summary>
        /// Returns a specific category by ID.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="id">The category ID.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid budgetPlanId, Guid id)
        {
            var category = await _budgetCategoryService.GetByIdAsync(id, budgetPlanId, GetUserId());
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Creates a new category within a budget plan.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        [HttpPost]
        public async Task<IActionResult> Create(Guid budgetPlanId, [FromBody] CreateBudgetCategoryDto dto)
        {
            var category = await _budgetCategoryService.CreateAsync(budgetPlanId, dto, GetUserId());
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Updates an existing budget category.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="id">The category ID.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid budgetPlanId, Guid id, [FromBody] UpdateBudgetCategoryDto dto)
        {
            var category = await _budgetCategoryService.UpdateAsync(id, budgetPlanId, dto, GetUserId());
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Deletes a budget category and all associated lines and transactions.
        /// </summary>
        /// <param name="budgetPlanId">The budget plan ID.</param>
        /// <param name="id">The category ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid budgetPlanId, Guid id)
        {
            var success = await _budgetCategoryService.DeleteAsync(id, budgetPlanId, GetUserId());
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
