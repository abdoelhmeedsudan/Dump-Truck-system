using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace DumpTruckManagementSystem.Application.Features.RevenueRateFeature.Commands.Create
{
    public class CreateRevenueRateCommandHandler : IRequestHandler<CreateRevenueRateCommand, Response<RevenueRateDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRevenueRateCommandHandler> _logger;

        public CreateRevenueRateCommandHandler(AppDbContext context, IMapper mapper, ILogger<CreateRevenueRateCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<RevenueRateDto>> Handle(CreateRevenueRateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _mapper.Map<RevenueRate>(request.body);
                entity.CreatedBy = request.userId.ToString();
                _context.RevenueRates.Add(entity);

                var created = await _context.SaveChangesAsync(cancellationToken);

                if (created > 0)
                    return new Response<RevenueRateDto>
                    {
                        Data = _mapper.Map<RevenueRateDto>(entity),
                        Succeeded = true,
                        Message = "Created Successfully",
                        HttpStatusCode = HttpStatusCode.Created
                    };

                throw new Exception("Error while creating RevenueRate");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying to create RevenueRate: {ex.Message} {ex.InnerException?.Message}");
                throw;
            }
        }
    }
}
