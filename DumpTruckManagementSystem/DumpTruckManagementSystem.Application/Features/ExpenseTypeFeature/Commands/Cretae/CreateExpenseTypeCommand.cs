using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Cretae
{
    public record CreateExpenseTypeCommand(Guid userId, CreateExpenseTypeDto body) : IRequest<Response<ExpenseTypeDto>>;
}

