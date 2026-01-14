using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Delete
{
    public record DeleteDumpTruckCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
