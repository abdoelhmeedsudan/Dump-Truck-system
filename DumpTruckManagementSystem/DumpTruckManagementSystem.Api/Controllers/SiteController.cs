using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Create;
using DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.SiteFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SiteController : BaseApiController<SiteController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] SiteParamDto param)
        {
            return await APIExecute(new GetAllSiteQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetSiteDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSiteDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateSiteCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSiteDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateSiteCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteSiteCommand(userId, id));
        }
    }
}
