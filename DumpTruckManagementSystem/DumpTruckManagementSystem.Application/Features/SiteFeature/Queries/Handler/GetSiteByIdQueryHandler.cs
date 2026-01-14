using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Application.Features.SiteFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.SiteFeature.Queries.Handler
{
    public class GetSiteByIdQueryHandler : IRequestHandler<GetSiteDtoByIdQuery, Response<SiteDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetSiteByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<SiteDto>> Handle(GetSiteDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var site = await _context.Set<Site>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (site == null)
            {
                return new Response<SiteDto>
                {
                    Succeeded = false,
                    Message = "Site not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<SiteDto>(site);

            return new Response<SiteDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
