using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Queries.Handler
{
    public class GetRevenueRateByIdQueryHandler : IRequestHandler<GetRevenueRateDtoByIdQuery, Response<RevenueRateDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetRevenueRateByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<RevenueRateDto>> Handle(GetRevenueRateDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var revenueRate = await _context.Set<RevenueRate>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (revenueRate == null)
            {
                return new Response<RevenueRateDto>
                {
                    Succeeded = false,
                    Message = "RevenueRate not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<RevenueRateDto>(revenueRate);

            return new Response<RevenueRateDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
