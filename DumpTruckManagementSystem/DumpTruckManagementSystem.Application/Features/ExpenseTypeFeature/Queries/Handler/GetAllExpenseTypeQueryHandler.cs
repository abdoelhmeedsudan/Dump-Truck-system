using AutoMapper;
using AutoMapper.QueryableExtensions;
using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Queries.Handler
{
    public class GetAllExpenseTypeQueryHandler : IRequestHandler<GetAllExpenseTypeQuery, Response<PagedList<ExpenseTypeDto>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetAllExpenseTypeQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<PagedList<ExpenseTypeDto>>> Handle(GetAllExpenseTypeQuery request, CancellationToken cancellationToken)
        {
            IQueryable<ExpenseType> query = _context.Set<ExpenseType>()
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
                .ProjectTo<ExpenseTypeDto>(_mapper.ConfigurationProvider);

            var data = await PagedList<ExpenseTypeDto>.ToPagedListAsync(
                projectedQuery,
                request.Param.PageNumber,
                request.Param.PageSize,
                cancellationToken);

            return new Response<PagedList<ExpenseTypeDto>>
            {
                Data = data,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
