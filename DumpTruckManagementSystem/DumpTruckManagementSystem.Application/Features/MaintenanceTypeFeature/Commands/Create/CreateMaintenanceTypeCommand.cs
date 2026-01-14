using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Create
{
    public record CreateMaintenanceTypeCommand(Guid userId, CreateMaintenanceTypeDto body) : IRequest<Response<MaintenanceTypeDto>>;
}
