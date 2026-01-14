using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos
{
    public class ShiftTruckEntryParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
        public Guid? ShiftId { get; set; }
        public Guid? DumpTruckId { get; set; }
    }
}
