using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Create
{
    public record CreateShiftTruckEntryCommand(Guid userId, CreateShiftTruckEntryDto body) : IRequest<Response<ShiftTruckEntryDto>>;
}
