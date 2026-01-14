using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Update
{
    public record UpdateMaintenanceRecordCommand(Guid userId, UpdateMaintenanceRecordDto body) : IRequest<Response<MaintenanceRecordDto>>;
}
