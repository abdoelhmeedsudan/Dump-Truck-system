using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Delete
{
    public record DeleteShiftExpenseCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
