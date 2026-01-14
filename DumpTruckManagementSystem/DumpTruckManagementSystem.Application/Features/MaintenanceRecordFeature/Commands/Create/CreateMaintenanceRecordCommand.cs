using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Create
{
    public record CreateMaintenanceRecordCommand(Guid userId, CreateMaintenanceRecordDto body) : IRequest<Response<MaintenanceRecordDto>>;
}
