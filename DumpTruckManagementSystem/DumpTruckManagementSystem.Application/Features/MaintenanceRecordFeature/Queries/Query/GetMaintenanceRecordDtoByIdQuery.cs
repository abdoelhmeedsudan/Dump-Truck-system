using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Queries.Query
{
    public record GetMaintenanceRecordDtoByIdQuery(Guid Id) : IRequest<Response<MaintenanceRecordDto>>;
}
