using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftTruckEntryFeature.Commands.Update
{
    public class UpdateShiftTruckEntryCommandHandler : IRequestHandler<UpdateShiftTruckEntryCommand, Response<ShiftTruckEntryDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateShiftTruckEntryCommandHandler> _logger;

        public UpdateShiftTruckEntryCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateShiftTruckEntryCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ShiftTruckEntryDto>> Handle(UpdateShiftTruckEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.ShiftTruckEntries
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<ShiftTruckEntryDto>
                    {
                        Succeeded = false,
                        Message = "ShiftTruckEntry not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.ShiftId = request.body.ShiftId;
                entity.DumpTruckId = request.body.DumpTruckId;
                entity.DriverId = request.body.DriverId;
                entity.TripsCount = request.body.TripsCount;
                entity.TripUnitPrice = request.body.TripUnitPrice;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.ShiftTruckEntries.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<ShiftTruckEntryDto>
                    {
                        Data = _mapper.Map<ShiftTruckEntryDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating ShiftTruckEntry");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update ShiftTruckEntry: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
