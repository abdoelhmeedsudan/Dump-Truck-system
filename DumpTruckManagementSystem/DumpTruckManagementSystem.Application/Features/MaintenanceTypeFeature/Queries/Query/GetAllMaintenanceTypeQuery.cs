using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Queries.Query
{
    public record GetAllMaintenanceTypeQuery(MaintenanceTypeParamDto Param) : IRequest<Response<PagedList<MaintenanceTypeDto>>>;
}
