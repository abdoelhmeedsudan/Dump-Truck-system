using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos
{
    public class RevenueRateParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
        public Guid? SiteId { get; set; }
    }
}
