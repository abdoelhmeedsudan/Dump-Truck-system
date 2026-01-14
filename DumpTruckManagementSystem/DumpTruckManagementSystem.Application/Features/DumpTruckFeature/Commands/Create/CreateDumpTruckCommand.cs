using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Create
{
    public record CreateDumpTruckCommand(Guid userId, CreateDumpTruckDto body) : IRequest<Response<DumpTruckDto>>;
}
