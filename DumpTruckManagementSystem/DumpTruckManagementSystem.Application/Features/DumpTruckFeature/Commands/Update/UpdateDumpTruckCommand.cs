using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Update
{
    public record UpdateDumpTruckCommand(Guid userId, UpdateDumpTruckDto body) : IRequest<Response<DumpTruckDto>>;
}
