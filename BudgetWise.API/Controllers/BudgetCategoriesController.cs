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

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid budgetPlanId)
        {
            var categories = await _budgetCategoryService.GetAllAsync(budgetPlanId, GetUserId());
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid budgetPlanId, Guid id)
        {
            var category = await _budgetCategoryService.GetByIdAsync(id, budgetPlanId, GetUserId());
            if (category == null)
                return NotFound();
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Guid budgetPlanId, [FromBody] CreateBudgetCategoryDto dto)
        {
            var category = await _budgetCategoryService.CreateAsync(budgetPlanId, dto, GetUserId());
            if (category == null)
                return NotFound();
            return Ok(category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid budgetPlanId, Guid id, [FromBody] UpdateBudgetCategoryDto dto)
        {
            var category = await _budgetCategoryService.UpdateAsync(id, budgetPlanId, dto, GetUserId());
            if (category == null)
                return NotFound();
            return Ok(category);
        }
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
