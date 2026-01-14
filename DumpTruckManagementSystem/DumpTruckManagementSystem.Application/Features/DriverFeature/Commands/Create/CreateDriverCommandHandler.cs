using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Create
{
    public class CreateDriverCommandHandler : IRequestHandler<CreateDriverCommand, Response<DriverDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateDriverCommandHandler> _logger;

        public CreateDriverCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateDriverCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<DriverDto>> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<Driver>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.Drivers.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<DriverDto>
                    {
                        Data = _mapper.Map<DriverDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating Driver");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create Driver: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
