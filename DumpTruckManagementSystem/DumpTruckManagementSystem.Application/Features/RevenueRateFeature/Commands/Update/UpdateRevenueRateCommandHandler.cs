using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Update
{
    public class UpdateRevenueRateCommandHandler : IRequestHandler<UpdateRevenueRateCommand, Response<RevenueRateDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRevenueRateCommandHandler> _logger;

        public UpdateRevenueRateCommandHandler(AppDbContext context, IMapper mapper, ILogger<UpdateRevenueRateCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<RevenueRateDto>> Handle(UpdateRevenueRateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _context.RevenueRates
                    .FirstOrDefaultAsync(x => x.Id == request.body.Id && !x.IsDeleted, cancellationToken);

                if (entity == null)
                    return new Response<RevenueRateDto>
                    {
                        Succeeded = false,
                        Message = "RevenueRate not found",
                        HttpStatusCode = HttpStatusCode.NotFound
                    };

                entity.SiteId = request.body.SiteId;
                entity.EffectiveFrom = request.body.EffectiveFrom;
                entity.RatePerTrip = request.body.RatePerTrip;
                entity.CurrencyCode = request.body.CurrencyCode;
                entity.ExchangeRateToSar = request.body.ExchangeRateToSar;
                entity.Notes = request.body.Notes;
                entity.EditAt = DateTime.Now;
                entity.EditBy = request.userId.ToString();

                _context.RevenueRates.Update(entity);
                var updated = await _context.SaveChangesAsync(cancellationToken);

                if (updated > 0)
                    return new Response<RevenueRateDto>
                    {
                        Data = _mapper.Map<RevenueRateDto>(entity),
                        Succeeded = true,
                        Message = "Updated Successfully",
                        HttpStatusCode = HttpStatusCode.OK
                    };

                throw new Exception("Error while updating RevenueRate");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to update RevenueRate: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
