using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Queries.Query
{
    public record GetAllExpenseTypeQuery(ExpenseTypeParamDto Param) : IRequest<Response<PagedList<ExpenseTypeDto>>>;
}
