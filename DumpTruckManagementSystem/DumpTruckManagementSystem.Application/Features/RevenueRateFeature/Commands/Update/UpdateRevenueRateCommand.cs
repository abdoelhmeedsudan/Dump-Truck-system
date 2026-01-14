using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Update
{
    public record UpdateRevenueRateCommand(Guid userId, UpdateRevenueRateDto body) : IRequest<Response<RevenueRateDto>>;
}
