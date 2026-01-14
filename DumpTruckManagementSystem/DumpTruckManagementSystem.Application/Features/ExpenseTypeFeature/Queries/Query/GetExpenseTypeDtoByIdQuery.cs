using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Queries.Query
{
    public record GetExpenseTypeDtoByIdQuery(Guid Id) : IRequest<Response<ExpenseTypeDto>>;
}
