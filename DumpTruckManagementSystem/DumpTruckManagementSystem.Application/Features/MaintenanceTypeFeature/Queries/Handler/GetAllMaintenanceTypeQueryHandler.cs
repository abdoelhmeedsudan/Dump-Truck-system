using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Queries.Handler
{
    public class GetAllMaintenanceTypeQueryHandler : IRequestHandler<GetAllMaintenanceTypeQuery, Response<PagedList<MaintenanceTypeDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllMaintenanceTypeQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<MaintenanceTypeDto>>> Handle(GetAllMaintenanceTypeQuery request, CancellationToken cancellationToken)
        {
            IQueryable<MaintenanceType> query = _context.Set<MaintenanceType>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.Param.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.Param.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Param.SearchTerm))
            {
                var search = request.Param.SearchTerm.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    (x.Notes != null && x.Notes.Contains(search)));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<MaintenanceTypeDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<MaintenanceTypeDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<MaintenanceTypeDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
