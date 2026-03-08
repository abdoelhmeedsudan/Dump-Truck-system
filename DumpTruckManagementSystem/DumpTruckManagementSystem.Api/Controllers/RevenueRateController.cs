using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class RevenueRateController : BaseApiController<RevenueRateController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] RevenueRateParamDto param)
        {
            return await APIExecute(new GetAllRevenueRateQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetRevenueRateDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRevenueRateDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateRevenueRateCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateRevenueRateDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateRevenueRateCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteRevenueRateCommand(userId, id));
        }
    }
}
