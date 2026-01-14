using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.ShiftFeature.Commands.Update
{
    public class UpdateShiftCommandHandler : IRequestHandler<UpdateShiftCommand, Response<ShiftDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateShiftCommandHandler> _logger;

        public UpdateShiftCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateShiftCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<ShiftDto>> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Shifts
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<ShiftDto>
                    {
                        Succeeded = false,
                        Message = "Shift not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.ShiftDate = request.body.ShiftDate;
                entity.SiteId = request.body.SiteId;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.Shifts.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<ShiftDto>
                    {
                        Data = _mapper.Map<ShiftDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating Shift");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update Shift: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
