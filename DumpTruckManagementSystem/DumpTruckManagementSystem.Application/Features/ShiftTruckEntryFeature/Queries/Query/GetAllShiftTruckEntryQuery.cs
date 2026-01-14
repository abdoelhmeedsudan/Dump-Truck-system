using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Queries.Query
{
    public record GetAllShiftTruckEntryQuery(ShiftTruckEntryParamDto Param) : IRequest<Response<PagedList<ShiftTruckEntryDto>>>;
}
