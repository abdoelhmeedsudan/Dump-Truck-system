namespace DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos
{
    public class CreateShiftExpenseDto
    {
        public Guid ShiftTruckEntryId { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
