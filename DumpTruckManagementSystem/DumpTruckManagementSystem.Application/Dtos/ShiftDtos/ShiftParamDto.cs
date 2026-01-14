using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.ShiftDtos
{
    public class ShiftParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
        public Guid? SiteId { get; set; }
    }
}
