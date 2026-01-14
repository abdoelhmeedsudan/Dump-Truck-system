using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Queries.Handler
{
    public class GetShiftExpenseByIdQueryHandler : IRequestHandler<GetShiftExpenseDtoByIdQuery, Response<ShiftExpenseDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetShiftExpenseByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ShiftExpenseDto>> Handle(GetShiftExpenseDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var shiftExpense = await _context.Set<ShiftExpense>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (shiftExpense == null)
            {
                return new Response<ShiftExpenseDto>
                {
                    Succeeded = false,
                    Message = "ShiftExpense not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<ShiftExpenseDto>(shiftExpense);

            return new Response<ShiftExpenseDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
