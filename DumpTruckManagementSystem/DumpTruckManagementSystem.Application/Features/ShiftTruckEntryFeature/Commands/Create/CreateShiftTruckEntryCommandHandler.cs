using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Create
{
    public class CreateShiftTruckEntryCommandHandler : IRequestHandler<CreateShiftTruckEntryCommand, Response<ShiftTruckEntryDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateShiftTruckEntryCommandHandler> _logger;

        public CreateShiftTruckEntryCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateShiftTruckEntryCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ShiftTruckEntryDto>> Handle(CreateShiftTruckEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<ShiftTruckEntry>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.ShiftTruckEntries.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<ShiftTruckEntryDto>
                    {
                        Data = _mapper.Map<ShiftTruckEntryDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating ShiftTruckEntry");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create ShiftTruckEntry: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
