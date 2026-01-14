using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Queries.Query
{
    public record GetAllShiftExpenseQuery(ShiftExpenseParamDto Param) : IRequest<Response<PagedList<ShiftExpenseDto>>>;
}
