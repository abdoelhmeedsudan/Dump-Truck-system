using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.ShiftFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class ShiftController : BaseApiController<ShiftController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ShiftParamDto param)
        {
            return await APIExecute(new GetAllShiftQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetShiftDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateShiftDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateShiftCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateShiftDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateShiftCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteShiftCommand(userId, id));
        }
    }
}
