using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Update
{
    public class UpdateMaintenanceRecordCommandHandler : IRequestHandler<UpdateMaintenanceRecordCommand, Response<MaintenanceRecordDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMaintenanceRecordCommandHandler> _logger;

        public UpdateMaintenanceRecordCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateMaintenanceRecordCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<MaintenanceRecordDto>> Handle(UpdateMaintenanceRecordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.MaintenanceRecords
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<MaintenanceRecordDto>
                    {
                        Succeeded = false,
                        Message = "MaintenanceRecord not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.MaintenanceDate = request.body.MaintenanceDate;
                entity.DumpTruckId = request.body.DumpTruckId;
                entity.MaintenanceTypeId = request.body.MaintenanceTypeId;
                entity.PartsCost = request.body.PartsCost;
                entity.LaborCost = request.body.LaborCost;
                entity.TotalCost = request.body.TotalCost;
                entity.DoneBy = request.body.DoneBy;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.MaintenanceRecords.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<MaintenanceRecordDto>
                    {
                        Data = _mapper.Map<MaintenanceRecordDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating MaintenanceRecord");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update MaintenanceRecord: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
