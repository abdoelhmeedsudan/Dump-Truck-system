using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Create
{
    public class CreateDumpTruckCommandHandler : IRequestHandler<CreateDumpTruckCommand, Response<DumpTruckDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateDumpTruckCommandHandler> _logger;

        public CreateDumpTruckCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateDumpTruckCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<DumpTruckDto>> Handle(CreateDumpTruckCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<DumpTruck>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.DumpTrucks.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<DumpTruckDto>
                    {
                        Data = _mapper.Map<DumpTruckDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating DumpTruck");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create DumpTruck: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
