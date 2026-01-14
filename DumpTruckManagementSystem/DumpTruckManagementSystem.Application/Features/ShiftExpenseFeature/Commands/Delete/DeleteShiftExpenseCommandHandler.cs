using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Delete
{
    public class DeleteShiftExpenseCommandHandler : IRequestHandler<DeleteShiftExpenseCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteShiftExpenseCommandHandler> _logger;

        public DeleteShiftExpenseCommandHandler(AppDbContext context, ILogger<DeleteShiftExpenseCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteShiftExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ShiftExpenses
                    .FirstOrDefaultAsync(x => x.Id == request.id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<bool>
                    {
                        Succeeded = false,
                        Message = "ShiftExpense not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.IsDeleted = true;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.ShiftExpenses.Update(entity);
                var deleted = await _context.SaveChangesAsync(cancellationToken);

                if (deleted > 0)
                    return new Response<bool>
                    {
                        Data = true,
                        Succeeded = true,
                        Message = "Deleted Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while deleting ShiftExpense");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to delete ShiftExpense: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
