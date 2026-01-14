using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Commands.Create
{
    public class CreateSiteCommandHandler : IRequestHandler<CreateSiteCommand, Response<SiteDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSiteCommandHandler> _logger;

        public CreateSiteCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateSiteCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<SiteDto>> Handle(CreateSiteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<Site>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.Sites.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<SiteDto>
                    {
                        Data = _mapper.Map<SiteDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating Site");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create Site: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
