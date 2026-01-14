using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Queries.Handler
{
    public class GetAllMaintenanceRecordQueryHandler : IRequestHandler<GetAllMaintenanceRecordQuery, Response<PagedList<MaintenanceRecordDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllMaintenanceRecordQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<MaintenanceRecordDto>>> Handle(GetAllMaintenanceRecordQuery request, CancellationToken cancellationToken)
        {
            IQueryable<MaintenanceRecord> query = _context.Set<MaintenanceRecord>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.Param.DumpTruckId.HasValue)
            {
                query = query.Where(x => x.DumpTruckId == request.Param.DumpTruckId.Value);
            }

            if (request.Param.MaintenanceTypeId.HasValue)
            {
                query = query.Where(x => x.MaintenanceTypeId == request.Param.MaintenanceTypeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Param.SearchTerm))
            {
                var search = request.Param.SearchTerm.Trim();
                query = query.Where(x =>
                    (x.DoneBy != null && x.DoneBy.Contains(search)) ||
                    (x.Notes != null && x.Notes.Contains(search)));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.MaintenanceDate)
                .ThenByDescending(x => x.CreatedAt)
                .ProjectTo<MaintenanceRecordDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<MaintenanceRecordDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<MaintenanceRecordDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
