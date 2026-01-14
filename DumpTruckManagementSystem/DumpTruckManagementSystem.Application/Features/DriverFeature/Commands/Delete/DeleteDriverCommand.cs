using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Delete
{
    public record DeleteDriverCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
