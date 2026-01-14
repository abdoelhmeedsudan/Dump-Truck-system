using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Queries.Query;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceRecordController : BaseApiController<MaintenanceRecordController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] MaintenanceRecordParamDto param)
        {
            return await APIExecute(new GetAllMaintenanceRecordQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetMaintenanceRecordDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMaintenanceRecordDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateMaintenanceRecordCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateMaintenanceRecordDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateMaintenanceRecordCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteMaintenanceRecordCommand(userId, id));
        }
    }
}
