using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Update
{
    public record UpdateExpenseTypeCommand(Guid userId, UpdateExpenseTypeDto body) : IRequest<Response<ExpenseTypeDto>>;
}
