using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Update
{
    public record UpdateMaintenanceTypeCommand(Guid userId, UpdateMaintenanceTypeDto body) : IRequest<Response<MaintenanceTypeDto>>;
}
