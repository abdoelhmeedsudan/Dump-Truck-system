using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Queries.Query
{
    public record GetShiftExpenseDtoByIdQuery(Guid Id) : IRequest<Response<ShiftExpenseDto>>;
}
