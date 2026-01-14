using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Update
{
    public record UpdateShiftTruckEntryCommand(Guid userId, UpdateShiftTruckEntryDto body) : IRequest<Response<ShiftTruckEntryDto>>;
}
