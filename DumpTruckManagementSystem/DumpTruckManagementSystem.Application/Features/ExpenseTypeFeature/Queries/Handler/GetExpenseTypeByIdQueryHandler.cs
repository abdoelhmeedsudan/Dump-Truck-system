using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Queries.Handler
{
    public class GetExpenseTypeByIdQueryHandler : IRequestHandler<GetExpenseTypeDtoByIdQuery, Response<ExpenseTypeDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetExpenseTypeByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<ExpenseTypeDto>> Handle(GetExpenseTypeDtoByIdQuery request, CancellationToken cancellationToken)
        {
            var expenseType = await _context.Set<ExpenseType>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

            if (expenseType == null)
            {
                return new Response<ExpenseTypeDto>
                {
                    Succeeded = false,
                    Message = "ExpenseType not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<ExpenseTypeDto>(expenseType);

            return new Response<ExpenseTypeDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
