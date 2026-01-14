using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Update
{
    public class UpdateShiftExpenseCommandHandler : IRequestHandler<UpdateShiftExpenseCommand, Response<ShiftExpenseDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateShiftExpenseCommandHandler> _logger;

        public UpdateShiftExpenseCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateShiftExpenseCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ShiftExpenseDto>> Handle(UpdateShiftExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ShiftExpenses
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<ShiftExpenseDto>
                    {
                        Succeeded = false,
                        Message = "ShiftExpense not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.ShiftTruckEntryId = request.body.ShiftTruckEntryId;
                entity.ExpenseTypeId = request.body.ExpenseTypeId;
                entity.Amount = request.body.Amount;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.ShiftExpenses.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<ShiftExpenseDto>
                    {
                        Data = _mapper.Map<ShiftExpenseDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating ShiftExpense");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update ShiftExpense: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
