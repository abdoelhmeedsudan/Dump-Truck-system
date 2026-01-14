using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Application.Features.DriverFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.DriverFeature.Queries.Handler
{
    public class GetDriverByIdQueryHandler: IRequestHandler<GetDriverDtoByIdQuery, Response<DriverDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetDriverByIdQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<DriverDto>> Handle(GetDriverDtoByIdQuery request,
            CancellationToken cancellationToken)
        {
            var driver = await _context.Set<Driver>()
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (driver == null)
            {
                return new Response<DriverDto>
                {
                    Succeeded = false,
                    Message = "Driver not found",
                    HttpStatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            var mappedResult = _mapper.Map<DriverDto>(driver);

            return new Response<DriverDto>
            {
                Data = mappedResult,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
