using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Queries.Query
{
    public record GetSiteDtoByIdQuery(Guid Id) : IRequest<Response<SiteDto>>;
}
