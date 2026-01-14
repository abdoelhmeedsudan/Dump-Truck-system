using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Delete
{
    public class DeleteDriverCommandHandler : IRequestHandler<DeleteDriverCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteDriverCommandHandler> _logger;

        public DeleteDriverCommandHandler(AppDbContext context, ILogger<DeleteDriverCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Drivers
                    .FirstOrDefaultAsync(x => x.Id == request.id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<bool>
                    {
                        Succeeded = false,
                        Message = "Driver not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.IsDeleted = true;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.Drivers.Update(entity);
                var deleted = await _context.SaveChangesAsync(cancellationToken);

                if (deleted > 0)
                    return new Response<bool>
                    {
                        Data = true,
                        Succeeded = true,
                        Message = "Deleted Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while deleting Driver");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to delete Driver: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
