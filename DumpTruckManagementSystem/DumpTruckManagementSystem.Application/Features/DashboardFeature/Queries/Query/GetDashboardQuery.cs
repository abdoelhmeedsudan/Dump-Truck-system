using DumpTruckManagementSystem.Application.Dtos.DashboardDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DashboardFeature.Queries.Query
{
    public record GetDashboardQuery : IRequest<Response<DashboardDto>>;
}
