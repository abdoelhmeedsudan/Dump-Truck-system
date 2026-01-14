using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ExpenseTypeFeature.Commands.Update
{
    public class UpdateExpenseTypeCommandHandler : IRequestHandler<UpdateExpenseTypeCommand, Response<ExpenseTypeDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateExpenseTypeCommandHandler> _logger;

        public UpdateExpenseTypeCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateExpenseTypeCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ExpenseTypeDto>> Handle(UpdateExpenseTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ExpenseTypes
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<ExpenseTypeDto>
                    {
                        Succeeded = false,
                        Message = "ExpenseType not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                // Update entity properties
                entity.Name = request.body.Name;
                entity.IsActive = request.body.IsActive;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.ExpenseTypes.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<ExpenseTypeDto>
                    {
                        Data = _mapper.Map<ExpenseTypeDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating ExpenseType");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update ExpenseType: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
