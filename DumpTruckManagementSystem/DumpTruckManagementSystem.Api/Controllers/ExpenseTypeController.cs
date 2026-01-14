using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Cretae;
using DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Delete;
using DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Update;
using DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Queries.Query;
using Microsoft.AspNetCore.Mvc;

namespace DumpTruckManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseTypeController : BaseApiController<ExpenseTypeController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] ExpenseTypeParamDto param)
        {
            return await APIExecute(new GetAllExpenseTypeQuery(param));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            return await APIExecute(new GetExpenseTypeDtoByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateExpenseTypeDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new CreateExpenseTypeCommand(userId, body));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateExpenseTypeDto body)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new UpdateExpenseTypeCommand(userId, body));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = Guid.TryParse(CurrentUserId, out var parsedUserId) ? parsedUserId : Guid.Empty;
            return await APIExecute(new DeleteExpenseTypeCommand(userId, id));
        }
    }
}
