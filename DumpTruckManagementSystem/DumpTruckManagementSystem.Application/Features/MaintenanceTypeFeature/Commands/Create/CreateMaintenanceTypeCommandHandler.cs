using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.MaintenanceTypeFeature.Commands.Create
{
    public class CreateMaintenanceTypeCommandHandler : IRequestHandler<CreateMaintenanceTypeCommand, Response<MaintenanceTypeDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMaintenanceTypeCommandHandler> _logger;

        public CreateMaintenanceTypeCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateMaintenanceTypeCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<MaintenanceTypeDto>> Handle(CreateMaintenanceTypeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<MaintenanceType>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.MaintenanceTypes.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<MaintenanceTypeDto>
                    {
                        Data = _mapper.Map<MaintenanceTypeDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating MaintenanceType");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create MaintenanceType: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
