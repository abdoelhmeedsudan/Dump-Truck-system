using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.SiteDtos
{
    public class SiteParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
    }
}
