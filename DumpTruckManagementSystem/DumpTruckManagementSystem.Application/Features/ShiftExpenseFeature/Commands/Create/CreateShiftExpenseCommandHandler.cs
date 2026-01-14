using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftExpenseFeature.Commands.Create
{
    public class CreateShiftExpenseCommandHandler : IRequestHandler<CreateShiftExpenseCommand, Response<ShiftExpenseDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateShiftExpenseCommandHandler> _logger;

        public CreateShiftExpenseCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateShiftExpenseCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ShiftExpenseDto>> Handle(CreateShiftExpenseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<ShiftExpense>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.ShiftExpenses.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<ShiftExpenseDto>
                    {
                        Data = _mapper.Map<ShiftExpenseDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating ShiftExpense");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create ShiftExpense: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
