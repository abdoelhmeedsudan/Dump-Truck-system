using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Delete
{
    public record DeleteExpenseTypeCommand(Guid userId, Guid id) : IRequest<Response<bool>>;
}
