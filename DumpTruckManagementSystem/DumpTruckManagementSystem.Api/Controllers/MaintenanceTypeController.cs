using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class MaintenanceTypeController : BaseApiController<MaintenanceTypeController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] MaintenanceTypeParamDto param)
        {
            return await APIExecute(new GetAllMaintenanceTypeQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetMaintenanceTypeDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateMaintenanceTypeDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateMaintenanceTypeCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateMaintenanceTypeDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateMaintenanceTypeCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteMaintenanceTypeCommand(userId, id));
        }
    }
}
