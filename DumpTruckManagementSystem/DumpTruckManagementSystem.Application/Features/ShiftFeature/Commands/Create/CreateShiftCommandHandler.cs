using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Create
{
    public class CreateShiftCommandHandler : IRequestHandler<CreateShiftCommand, Response<ShiftDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateShiftCommandHandler> _logger;

        public CreateShiftCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateShiftCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ShiftDto>> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<Shift>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.Shifts.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<ShiftDto>
                    {
                        Data = _mapper.Map<ShiftDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating Shift");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create Shift: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
