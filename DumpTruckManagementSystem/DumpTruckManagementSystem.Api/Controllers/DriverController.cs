using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.DriverFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DriverController : BaseApiController<DriverController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] DriverParamDto param)
        {
            return await APIExecute(new GetAllDriverQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetDriverDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDriverDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateDriverCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDriverDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateDriverCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteDriverCommand(userId, id));
        }
    }
}
