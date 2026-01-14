using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Application.Features.ShiftFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Queries.Handler
{
    public class GetShiftByIdQueryHandler : IRequestHandler<GetShiftDtoByIdQuery, Response<ShiftDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetShiftByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ShiftDto>> Handle(GetShiftDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var shift = await _context.Set<Shift>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (shift == null)
            {
                return new Response<ShiftDto>
                {
                    Succeeded = false,
                    Message = "Shift not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<ShiftDto>(shift);

            return new Response<ShiftDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
