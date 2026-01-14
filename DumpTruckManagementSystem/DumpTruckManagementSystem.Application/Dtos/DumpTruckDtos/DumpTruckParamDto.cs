using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos
{
    public class DumpTruckParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
    }
}
