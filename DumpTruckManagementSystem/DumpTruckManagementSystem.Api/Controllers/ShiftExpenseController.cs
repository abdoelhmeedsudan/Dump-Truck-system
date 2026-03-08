using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ShiftExpenseController : BaseApiController<ShiftExpenseController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ShiftExpenseParamDto param)
        {
            return await APIExecute(new GetAllShiftExpenseQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetShiftExpenseDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateShiftExpenseDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateShiftExpenseCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateShiftExpenseDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateShiftExpenseCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteShiftExpenseCommand(userId, id));
        }
    }
}
