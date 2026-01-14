using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Delete
{
    public record DeleteSiteCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
