using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Update
{
    public class UpdateMaintenanceTypeCommandHandler : IRequestHandler<UpdateMaintenanceTypeCommand, Response<MaintenanceTypeDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMaintenanceTypeCommandHandler> _logger;

        public UpdateMaintenanceTypeCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateMaintenanceTypeCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<MaintenanceTypeDto>> Handle(UpdateMaintenanceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.MaintenanceTypes
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<MaintenanceTypeDto>
                    {
                        Succeeded = false,
                        Message = "MaintenanceType not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.Name = request.body.Name;
                entity.IsActive = request.body.IsActive;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.MaintenanceTypes.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<MaintenanceTypeDto>
                    {
                        Data = _mapper.Map<MaintenanceTypeDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating MaintenanceType");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update MaintenanceType: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
