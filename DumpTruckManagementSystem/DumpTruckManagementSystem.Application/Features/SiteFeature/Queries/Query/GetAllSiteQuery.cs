using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Queries.Query
{
    public record GetAllSiteQuery(SiteParamDto Param) : IRequest<Response<PagedList<SiteDto>>>;
}
