using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Queries.Handler
{
    public class GetMaintenanceTypeByIdQueryHandler : IRequestHandler<GetMaintenanceTypeDtoByIdQuery, Response<MaintenanceTypeDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetMaintenanceTypeByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<MaintenanceTypeDto>> Handle(GetMaintenanceTypeDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var maintenanceType = await _context.Set<MaintenanceType>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (maintenanceType == null)
            {
                return new Response<MaintenanceTypeDto>
                {
                    Succeeded = false,
                    Message = "MaintenanceType not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<MaintenanceTypeDto>(maintenanceType);

            return new Response<MaintenanceTypeDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
