using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Update
{
    public record UpdateDriverCommand(Guid userId, UpdateDriverDto body) : IRequest<Response<DriverDto>>;
}
