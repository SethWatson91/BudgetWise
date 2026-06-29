using BudgetWise.Core.DTOs;
using BudgetWise.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetWise.API.Controllers
{
    [ApiController]
    [Route("api/v1/budget-plans")]
    [Authorize]
    public class BudgetPlansController : ControllerBase
    {
        private readonly BudgetPlanService _budgetPlanService;

        public BudgetPlansController(BudgetPlanService budgetPlanService)
        {
            _budgetPlanService = budgetPlanService;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Returns all budget plans for the authenticated user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var plans = await _budgetPlanService.GetAllAsync(GetUserId());
            return Ok(plans);
        }

        /// <summary>
        /// Returns a specific budget plan by ID.
        /// </summary>
        /// <param name="id">The budget plan ID.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var plan = await _budgetPlanService.GetByIdAsync(id, GetUserId());
            if (plan == null)
                return NotFound();
            return Ok(plan);
        }

        /// <summary>
        /// Creates a new monthly budget plan for the authenticated user.
        /// </summary>
        /// <remarks>Only one plan is allowed per user per month and year.</remarks>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBudgetPlanDto dto)
        {
            try
            {
                var plan = await _budgetPlanService.CreateAsync(dto, GetUserId());
                return Ok(plan);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message =  ex.Message });
            }
        }

        /// <summary>
        /// Updates the total income for an existing budget plan.
        /// </summary>
        /// <param name="id">The budget plan ID.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id,  [FromBody] UpdateBudgetPlanDto dto)
        {
            var plan = await _budgetPlanService.UpdateAsync(id, dto, GetUserId());
            if (plan == null)
                return NotFound();
            return Ok(plan);
        }

        /// <summary>
        /// Deletes a budget plan and all associated categories, lines, and transactions.
        /// </summary>
        /// <param name="id">The budget plan ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _budgetPlanService.DeleteAsync(id, GetUserId());
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
