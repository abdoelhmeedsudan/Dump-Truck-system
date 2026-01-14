using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Queries.Query
{
    public record GetDumpTruckDtoByIdQuery(Guid Id) : IRequest<Response<DumpTruckDto>>;
}
