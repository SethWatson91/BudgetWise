using BudgetWise.Core.DTOs;
using BudgetWise.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BudgetWise.API.Controllers
{
    [ApiController]
    [Route("api/budgetplans/{budgetPlanId}/categories/{budgetCategoryId}/lines")]
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

        [HttpGet]
        public async Task<IActionResult> GetAll(Guid budgetPlanId, Guid budgetCategoryId)
        {
            var budgetLines = await _budgetLineService.GetAllAsync(budgetCategoryId, budgetPlanId, GetUserId());
            return Ok(budgetLines);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById( Guid budgetPlanId, Guid budgetCategoryId, Guid id)
        {
            var budgetLine = await _budgetLineService.GetByIdAsync(id, budgetCategoryId, budgetPlanId, GetUserId());
            if (budgetLine == null)
                return NotFound();
            return Ok(budgetLine);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Guid budgetPlanId, Guid budgetCategoryId, [FromBody] CreateBudgetLineDto dto)
        {
            var budgetLine = await _budgetLineService.CreateAsync(budgetCategoryId, budgetPlanId, dto, GetUserId());
            if (budgetLine == null)
                return NotFound();
            return Ok(budgetLine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid budgetPlanId, Guid budgetCategoryId, Guid id, [FromBody] UpdateBudgetLineDto dto)
        {
            var budgetLine = await _budgetLineService.UpdateAsync(id, budgetCategoryId, budgetPlanId, dto, GetUserId());
            if (budgetLine == null)
                return NotFound();
            return Ok(budgetLine);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid budgetPlanId, Guid budgetCategoryId, Guid id)
        {
            var success = await _budgetLineService.DeleteAsync(id, budgetCategoryId, budgetPlanId, GetUserId());
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
