using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceRecordFeature.Commands.Create
{
    public class CreateMaintenanceRecordCommandHandler : IRequestHandler<CreateMaintenanceRecordCommand, Response<MaintenanceRecordDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMaintenanceRecordCommandHandler> _logger;

        public CreateMaintenanceRecordCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateMaintenanceRecordCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<MaintenanceRecordDto>> Handle(CreateMaintenanceRecordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<MaintenanceRecord>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.MaintenanceRecords.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<MaintenanceRecordDto>
                    {
                        Data = _mapper.Map<MaintenanceRecordDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating MaintenanceRecord");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create MaintenanceRecord: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
