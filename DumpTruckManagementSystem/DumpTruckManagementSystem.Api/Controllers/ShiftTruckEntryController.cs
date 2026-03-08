using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShiftTruckEntryController : BaseApiController<ShiftTruckEntryController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ShiftTruckEntryParamDto param)
        {
            return await APIExecute(new GetAllShiftTruckEntryQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetShiftTruckEntryDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateShiftTruckEntryDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateShiftTruckEntryCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateShiftTruckEntryDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateShiftTruckEntryCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteShiftTruckEntryCommand(userId, id));
        }
    }
}
