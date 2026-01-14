using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Update
{
    public class UpdateSiteCommandHandler : IRequestHandler<UpdateSiteCommand, Response<SiteDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSiteCommandHandler> _logger;

        public UpdateSiteCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateSiteCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<SiteDto>> Handle(UpdateSiteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Sites
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<SiteDto>
                    {
                        Succeeded = false,
                        Message = "Site not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.Name = request.body.Name;
                entity.Code = request.body.Code;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.Sites.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<SiteDto>
                    {
                        Data = _mapper.Map<SiteDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating Site");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update Site: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
