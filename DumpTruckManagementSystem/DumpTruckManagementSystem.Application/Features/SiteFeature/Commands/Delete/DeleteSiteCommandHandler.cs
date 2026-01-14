using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Delete
{
    public class DeleteSiteCommandHandler : IRequestHandler<DeleteSiteCommand, Response<bool>>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteSiteCommandHandler> _logger;

        public DeleteSiteCommandHandler(AppDbContext context, ILogger<DeleteSiteCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Response<bool>> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Sites
                    .FirstOrDefaultAsync(x => x.Id == request.id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<bool>
                    {
                        Succeeded = false,
                        Message = "Site not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.IsDeleted = true;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.Sites.Update(entity);
                var deleted = await _context.SaveChangesAsync(cancellationToken);

                if (deleted > 0)
                    return new Response<bool>
                    {
                        Data = true,
                        Succeeded = true,
                        Message = "Deleted Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while deleting Site");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to delete Site: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
