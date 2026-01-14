using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos
{
    public class ExpenseTypeParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
        public bool? IsActive { get; set; }
    }
}
