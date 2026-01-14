using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Queries.Handler
{
    public class GetAllShiftTruckEntryQueryHandler : IRequestHandler<GetAllShiftTruckEntryQuery, Response<PagedList<ShiftTruckEntryDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllShiftTruckEntryQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<ShiftTruckEntryDto>>> Handle(GetAllShiftTruckEntryQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ShiftTruckEntry> query = _context.Set<ShiftTruckEntry>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.Param.ShiftId.HasValue)
            {
                query = query.Where(x => x.ShiftId == request.Param.ShiftId.Value);
            }

            if (request.Param.DumpTruckId.HasValue)
            {
                query = query.Where(x => x.DumpTruckId == request.Param.DumpTruckId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Param.SearchTerm))
            {
                var search = request.Param.SearchTerm.Trim();
                query = query.Where(x => x.Notes != null && x.Notes.Contains(search));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<ShiftTruckEntryDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<ShiftTruckEntryDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<ShiftTruckEntryDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
