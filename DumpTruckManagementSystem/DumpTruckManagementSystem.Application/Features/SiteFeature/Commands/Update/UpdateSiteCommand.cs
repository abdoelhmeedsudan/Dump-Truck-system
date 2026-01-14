using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Update
{
    public record UpdateSiteCommand(Guid userId, UpdateSiteDto body) : IRequest<Response<SiteDto>>;
}
