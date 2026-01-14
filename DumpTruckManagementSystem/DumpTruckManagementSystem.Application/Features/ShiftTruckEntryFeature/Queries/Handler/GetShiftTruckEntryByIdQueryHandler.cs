using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Queries.Handler
{
    public class GetShiftTruckEntryByIdQueryHandler : IRequestHandler<GetShiftTruckEntryDtoByIdQuery, Response<ShiftTruckEntryDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetShiftTruckEntryByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ShiftTruckEntryDto>> Handle(GetShiftTruckEntryDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var shiftTruckEntry = await _context.Set<ShiftTruckEntry>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (shiftTruckEntry == null)
            {
                return new Response<ShiftTruckEntryDto>
                {
                    Succeeded = false,
                    Message = "ShiftTruckEntry not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<ShiftTruckEntryDto>(shiftTruckEntry);

            return new Response<ShiftTruckEntryDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
