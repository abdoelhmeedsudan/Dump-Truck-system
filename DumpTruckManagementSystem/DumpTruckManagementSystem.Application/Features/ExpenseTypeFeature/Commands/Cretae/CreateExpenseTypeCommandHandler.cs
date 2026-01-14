using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Cretae
{
    public class CreateExpenseTypeCommandHandler : IRequestHandler<CreateExpenseTypeCommand, Response<ExpenseTypeDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateExpenseTypeCommandHandler> _logger;

        public CreateExpenseTypeCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateExpenseTypeCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ExpenseTypeDto>> Handle(CreateExpenseTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {

                // Code to perform the main action
                var entity = _mapper.Map<ExpenseType>(request.body);
                _context.ExpenseTypes.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<ExpenseTypeDto>
                    {
                        Data = _mapper.Map<ExpenseTypeDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating ExpenseType");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create ExpenseType: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }

    }
}

