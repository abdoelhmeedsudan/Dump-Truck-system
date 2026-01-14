using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Delete
{
    public class DeleteRevenueRateCommandHandler : IRequestHandler<DeleteRevenueRateCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteRevenueRateCommandHandler> _logger;

        public DeleteRevenueRateCommandHandler(AppDbContext context, ILogger<DeleteRevenueRateCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteRevenueRateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.RevenueRates
                    .FirstOrDefaultAsync(x => x.Id == request.id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<bool>
                    {
                        Succeeded = false,
                        Message = "RevenueRate not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.IsDeleted = true;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.RevenueRates.Update(entity);
                var deleted = await _context.SaveChangesAsync(cancellationToken);

                if (deleted > 0)
                    return new Response<bool>
                    {
                        Data = true,
                        Succeeded = true,
                        Message = "Deleted Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while deleting RevenueRate");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to delete RevenueRate: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
