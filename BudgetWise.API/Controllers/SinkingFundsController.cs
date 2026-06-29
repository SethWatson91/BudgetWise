using BudgetWise.Core.DTOs;
using BudgetWise.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BudgetWise.API.Controllers
{
    [ApiController]
    [Route("api/v1/sinking-funds")]
    [Authorize]
    public class SinkingFundsController : ControllerBase
    {
        private readonly SinkingFundService _sinkingFundService;
        public SinkingFundsController(SinkingFundService sinkingFundService)
        {
            _sinkingFundService = sinkingFundService;
        }
        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Returns all sinking funds for the authenticated user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var sinkingFunds = await _sinkingFundService.GetAllAsync(GetUserId());
            return Ok(sinkingFunds);
        }

        /// <summary>
        /// Returns a specific sinking fund by ID.
        /// </summary>
        /// <param name="id">The sinking fund ID.</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var sinkingFund = await _sinkingFundService.GetByIdAsync(id, GetUserId());
            if (sinkingFund == null)
                return NotFound();
            return Ok(sinkingFund);
        }

        /// <summary>
        /// Creates a new sinking fund for the authenticated user.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSinkingFundDto dto)
        {
            var sinkingFund = await _sinkingFundService.CreateAsync(dto, GetUserId());
            return Ok(sinkingFund);
        }

        /// <summary>
        /// Updates an existing sinking fund.
        /// </summary>
        /// <param name="id">The sinking fund ID.</param>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSinkingFundDto dto)
        {
            var sinkingFund = await _sinkingFundService.UpdateAsync(id, dto, GetUserId());
            if (sinkingFund == null)
                return NotFound();
            return Ok(sinkingFund);
        }

        /// <summary>
        /// Deletes a sinking fund.
        /// </summary>
        /// <param name="id">The sinking fund ID.</param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _sinkingFundService.DeleteAsync(id, GetUserId());
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
