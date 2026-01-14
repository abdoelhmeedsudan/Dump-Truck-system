using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Delete
{
    public class DeleteMaintenanceTypeCommandHandler : IRequestHandler<DeleteMaintenanceTypeCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteMaintenanceTypeCommandHandler> _logger;

        public DeleteMaintenanceTypeCommandHandler(AppDbContext context, ILogger<DeleteMaintenanceTypeCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteMaintenanceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.MaintenanceTypes
                    .FirstOrDefaultAsync(x => x.Id == request.id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<bool>
                    {
                        Succeeded = false,
                        Message = "MaintenanceType not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.IsDeleted = true;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.MaintenanceTypes.Update(entity);
                var deleted = await _context.SaveChangesAsync(cancellationToken);

                if (deleted > 0)
                    return new Response<bool>
                    {
                        Data = true,
                        Succeeded = true,
                        Message = "Deleted Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while deleting MaintenanceType");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to delete MaintenanceType: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
