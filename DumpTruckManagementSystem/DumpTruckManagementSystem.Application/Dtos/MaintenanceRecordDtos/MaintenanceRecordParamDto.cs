using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos
{
    public class MaintenanceRecordParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
        public Guid? DumpTruckId { get; set; }
        public Guid? MaintenanceTypeId { get; set; }
    }
}
