using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Create
{
    public record CreateDriverCommand(Guid userId, CreateDriverDto body) : IRequest<Response<DriverDto>>;
}
