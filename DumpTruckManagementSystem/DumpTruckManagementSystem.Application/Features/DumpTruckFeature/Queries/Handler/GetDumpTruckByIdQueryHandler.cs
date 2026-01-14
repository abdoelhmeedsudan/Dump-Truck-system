using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Queries.Handler
{
    public class GetDumpTruckByIdQueryHandler : IRequestHandler<GetDumpTruckDtoByIdQuery, Response<DumpTruckDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetDumpTruckByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<DumpTruckDto>> Handle(GetDumpTruckDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var dumpTruck = await _context.Set<DumpTruck>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (dumpTruck == null)
            {
                return new Response<DumpTruckDto>
                {
                    Succeeded = false,
                    Message = "DumpTruck not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<DumpTruckDto>(dumpTruck);

            return new Response<DumpTruckDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
