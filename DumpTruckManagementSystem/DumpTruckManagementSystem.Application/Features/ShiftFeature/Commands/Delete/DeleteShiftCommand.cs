using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Delete
{
    public record DeleteShiftCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
