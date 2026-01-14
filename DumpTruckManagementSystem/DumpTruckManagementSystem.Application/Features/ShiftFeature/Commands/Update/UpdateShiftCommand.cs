using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Update
{
    public record UpdateShiftCommand(Guid userId, UpdateShiftDto body) : IRequest<Response<ShiftDto>>;
}
