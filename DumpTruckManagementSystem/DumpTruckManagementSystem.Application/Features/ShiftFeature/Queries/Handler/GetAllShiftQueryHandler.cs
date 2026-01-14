using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Application.Features.ShiftFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Queries.Handler
{
    public class GetAllShiftQueryHandler : IRequestHandler<GetAllShiftQuery, Response<PagedList<ShiftDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllShiftQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<ShiftDto>>> Handle(GetAllShiftQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Shift> query = _context.Set<Shift>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.Param.SiteId.HasValue)
            {
                query = query.Where(x => x.SiteId == request.Param.SiteId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Param.SearchTerm))
            {
                var search = request.Param.SearchTerm.Trim();
                query = query.Where(x => x.Notes != null && x.Notes.Contains(search));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.ShiftDate)
                .ThenByDescending(x => x.CreatedAt)
                .ProjectTo<ShiftDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<ShiftDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<ShiftDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
