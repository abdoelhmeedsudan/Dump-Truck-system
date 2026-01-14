using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Create
{
    public record CreateShiftCommand(Guid userId, CreateShiftDto body) : IRequest<Response<ShiftDto>>;
}
