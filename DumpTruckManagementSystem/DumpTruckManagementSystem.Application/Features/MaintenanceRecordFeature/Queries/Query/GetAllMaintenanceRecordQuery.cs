using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Queries.Query
{
    public record GetAllMaintenanceRecordQuery(MaintenanceRecordParamDto Param) : IRequest<Response<PagedList<MaintenanceRecordDto>>>;
}
