using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Delete
{
    public class DeleteShiftTruckEntryCommandHandler : IRequestHandler<DeleteShiftTruckEntryCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteShiftTruckEntryCommandHandler> _logger;

        public DeleteShiftTruckEntryCommandHandler(AppDbContext context, ILogger<DeleteShiftTruckEntryCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteShiftTruckEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ShiftTruckEntries
                    .FirstOrDefaultAsync(x => x.Id == request.id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<bool>
                    {
                        Succeeded = false,
                        Message = "ShiftTruckEntry not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.IsDeleted = true;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.ShiftTruckEntries.Update(entity);
                var deleted = await _context.SaveChangesAsync(cancellationToken);

                if (deleted > 0)
                    return new Response<bool>
                    {
                        Data = true,
                        Succeeded = true,
                        Message = "Deleted Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while deleting ShiftTruckEntry");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to delete ShiftTruckEntry: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
