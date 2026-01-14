using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Create
{
    public record CreateShiftExpenseCommand(Guid userId, CreateShiftExpenseDto body) : IRequest<Response<ShiftExpenseDto>>;
}
