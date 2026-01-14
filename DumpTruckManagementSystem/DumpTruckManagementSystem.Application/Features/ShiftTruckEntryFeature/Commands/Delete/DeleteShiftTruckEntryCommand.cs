using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Delete
{
    public record DeleteShiftTruckEntryCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
