using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Create
{
    public record CreateRevenueRateCommand(Guid userId, CreateRevenueRateDto body) : IRequest<Response<RevenueRateDto>>;
}
