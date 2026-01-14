using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Queries.Query
{
    public record GetRevenueRateDtoByIdQuery(Guid Id) : IRequest<Response<RevenueRateDto>>;
}
