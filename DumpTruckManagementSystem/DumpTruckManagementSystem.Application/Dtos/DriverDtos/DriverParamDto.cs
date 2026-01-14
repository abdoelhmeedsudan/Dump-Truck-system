using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.DriverDtos
{
    public class DriverParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }

        public bool IsActive { get; set; }
    }
}
