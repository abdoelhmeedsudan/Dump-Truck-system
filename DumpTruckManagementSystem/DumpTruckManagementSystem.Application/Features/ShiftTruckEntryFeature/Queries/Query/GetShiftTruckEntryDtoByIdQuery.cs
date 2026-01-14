using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Queries.Query
{
    public record GetShiftTruckEntryDtoByIdQuery(Guid Id) : IRequest<Response<ShiftTruckEntryDto>>;
}
