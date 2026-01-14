using DumpTruckManagementSystem.Shared.Wrappers;

namespace DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos
{
    public class ShiftExpenseParamDto : PaginationParamsDto
    {
        public string? SearchTerm { get; set; }
        public Guid? ShiftTruckEntryId { get; set; }
        public Guid? ExpenseTypeId { get; set; }
    }
}
