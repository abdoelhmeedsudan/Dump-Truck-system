using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Queries.Handler
{
    public class GetAllDumpTruckQueryHandler : IRequestHandler<GetAllDumpTruckQuery, Response<PagedList<DumpTruckDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllDumpTruckQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<DumpTruckDto>>> Handle(GetAllDumpTruckQuery request, CancellationToken cancellationToken)
        {
            IQueryable<DumpTruck> query = _context.Set<DumpTruck>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Param.SearchTerm))
            {
                var search = request.Param.SearchTerm.Trim();

                query = query.Where(x =>
                    x.TruckNumber.Contains(search) ||
                    x.PlateNumber.Contains(search) ||
                    (x.Model != null && x.Model.Contains(search)));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<DumpTruckDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<DumpTruckDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<DumpTruckDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
