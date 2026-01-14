using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Delete
{
    public record DeleteRevenueRateCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
