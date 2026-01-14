using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos
{
    public class MaintenanceTypeParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
    }
}
