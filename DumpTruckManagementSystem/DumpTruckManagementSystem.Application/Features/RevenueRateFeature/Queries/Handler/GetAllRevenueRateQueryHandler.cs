using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Queries.Handler
{
    public class GetAllRevenueRateQueryHandler : IRequestHandler<GetAllRevenueRateQuery, Response<PagedList<RevenueRateDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllRevenueRateQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<RevenueRateDto>>> Handle(GetAllRevenueRateQuery request, CancellationToken cancellationToken)
        {
            IQueryable<RevenueRate> query = _context.Set<RevenueRate>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.Param.SiteId.HasValue)
            {
                query = query.Where(x => x.SiteId == request.Param.SiteId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Param.SearchTerm))
            {
                var search = request.Param.SearchTerm.Trim();
                query = query.Where(x =>
                    x.CurrencyCode.Contains(search) ||
                    (x.Notes != null && x.Notes.Contains(search)));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.EffectiveFrom)
                .ThenByDescending(x => x.CreatedAt)
                .ProjectTo<RevenueRateDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<RevenueRateDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<RevenueRateDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
