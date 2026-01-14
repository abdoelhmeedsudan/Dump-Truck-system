using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Delete
{
    public record DeleteMaintenanceRecordCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
