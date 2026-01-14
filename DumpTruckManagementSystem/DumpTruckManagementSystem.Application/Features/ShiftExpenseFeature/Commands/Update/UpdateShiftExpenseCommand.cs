using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Update
{
    public record UpdateShiftExpenseCommand(Guid userId, UpdateShiftExpenseDto body) : IRequest<Response<ShiftExpenseDto>>;
}
