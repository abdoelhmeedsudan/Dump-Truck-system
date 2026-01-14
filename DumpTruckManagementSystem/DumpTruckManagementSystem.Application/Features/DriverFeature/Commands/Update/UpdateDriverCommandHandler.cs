using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Commands.Update
{
    public class UpdateDriverCommandHandler : IRequestHandler<UpdateDriverCommand, Response<DriverDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateDriverCommandHandler> _logger;

        public UpdateDriverCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateDriverCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<DriverDto>> Handle(UpdateDriverCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.Drivers
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<DriverDto>
                    {
                        Succeeded = false,
                        Message = "Driver not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.FullName = request.body.FullName;
                entity.PhoneNumber = request.body.PhoneNumber;
                entity.NationalId = request.body.NationalId;
                entity.IsActive = request.body.IsActive;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.Drivers.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<DriverDto>
                    {
                        Data = _mapper.Map<DriverDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating Driver");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update Driver: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
