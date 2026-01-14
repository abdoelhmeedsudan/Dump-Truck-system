using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Queries.Query
{
    public record GetDriverDtoByIdQuery(Guid Id) : IRequest<Response<DriverDto>>;

}
