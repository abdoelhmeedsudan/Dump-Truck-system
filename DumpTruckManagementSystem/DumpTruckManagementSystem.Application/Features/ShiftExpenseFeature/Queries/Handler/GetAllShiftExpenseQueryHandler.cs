using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Queries.Handler
{
    public class GetAllShiftExpenseQueryHandler : IRequestHandler<GetAllShiftExpenseQuery, Response<PagedList<ShiftExpenseDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllShiftExpenseQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<ShiftExpenseDto>>> Handle(GetAllShiftExpenseQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ShiftExpense> query = _context.Set<ShiftExpense>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (request.Param.ShiftTruckEntryId.HasValue)
            {
                query = query.Where(x => x.ShiftTruckEntryId == request.Param.ShiftTruckEntryId.Value);
            }

            if (request.Param.ExpenseTypeId.HasValue)
            {
                query = query.Where(x => x.ExpenseTypeId == request.Param.ExpenseTypeId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Param.SearchTerm))
            {
                var search = request.Param.SearchTerm.Trim();
                query = query.Where(x => x.Notes != null && x.Notes.Contains(search));
            }

            var projectedQuery = query
                .OrderByDescending(x => x.CreatedAt)
                .ProjectTo<ShiftExpenseDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<ShiftExpenseDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<ShiftExpenseDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
