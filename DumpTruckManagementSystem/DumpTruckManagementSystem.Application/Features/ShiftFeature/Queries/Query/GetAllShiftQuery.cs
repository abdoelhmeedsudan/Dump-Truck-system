using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Queries.Query
{
    public record GetAllShiftQuery(ShiftParamDto Param) : IRequest<Response<PagedList<ShiftDto>>>;
}
