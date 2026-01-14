using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.DumpTruckFeature.Commands.Update
{
    public class UpdateDumpTruckCommandHandler : IRequestHandler<UpdateDumpTruckCommand, Response<DumpTruckDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateDumpTruckCommandHandler> _logger;

        public UpdateDumpTruckCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateDumpTruckCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<DumpTruckDto>> Handle(UpdateDumpTruckCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.DumpTrucks
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<DumpTruckDto>
                    {
                        Succeeded = false,
                        Message = "DumpTruck not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.TruckNumber = request.body.TruckNumber;
                entity.PlateNumber = request.body.PlateNumber;
                entity.TruckType = request.body.TruckType;
                entity.Model = request.body.Model;
                entity.LoadCapacity = request.body.LoadCapacity;
                entity.Status = request.body.Status;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.DumpTrucks.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<DumpTruckDto>
                    {
                        Data = _mapper.Map<DumpTruckDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating DumpTruck");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update DumpTruck: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
