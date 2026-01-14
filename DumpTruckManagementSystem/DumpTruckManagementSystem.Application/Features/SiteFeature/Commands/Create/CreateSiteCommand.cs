using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Create
{
    public record CreateSiteCommand(Guid userId, CreateSiteDto body) : IRequest<Response<SiteDto>>;
}
