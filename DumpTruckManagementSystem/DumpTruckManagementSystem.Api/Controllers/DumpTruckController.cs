using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Queries.Query;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DumpTruckController : BaseApiController<DumpTruckController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] DumpTruckParamDto param)
        {
            return await APIExecute(new GetAllDumpTruckQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetDumpTruckDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateDumpTruckDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateDumpTruckCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateDumpTruckDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateDumpTruckCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteDumpTruckCommand(userId, id));
        }
    }
}
