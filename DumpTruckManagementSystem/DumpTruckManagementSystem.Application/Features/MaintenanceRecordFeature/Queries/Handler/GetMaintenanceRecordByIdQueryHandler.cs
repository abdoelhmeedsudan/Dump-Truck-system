using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Queries.Handler
{
    public class GetMaintenanceRecordByIdQueryHandler : IRequestHandler<GetMaintenanceRecordDtoByIdQuery, Response<MaintenanceRecordDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetMaintenanceRecordByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<MaintenanceRecordDto>> Handle(GetMaintenanceRecordDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var maintenanceRecord = await _context.Set<MaintenanceRecord>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (maintenanceRecord == null)
            {
                return new Response<MaintenanceRecordDto>
                {
                    Succeeded = false,
                    Message = "MaintenanceRecord not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<MaintenanceRecordDto>(maintenanceRecord);

            return new Response<MaintenanceRecordDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
