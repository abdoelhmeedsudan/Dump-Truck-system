using DumpTruckManagementSystem.Application.Features.DashboardFeature.Queries.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : BaseApiController<DashboardController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return await APIExecute(new GetDashboardQuery());
        }
    }
}
