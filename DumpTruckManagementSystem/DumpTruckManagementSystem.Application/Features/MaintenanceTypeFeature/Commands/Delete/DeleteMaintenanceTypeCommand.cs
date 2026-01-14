using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Delete
{
    public record DeleteMaintenanceTypeCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
