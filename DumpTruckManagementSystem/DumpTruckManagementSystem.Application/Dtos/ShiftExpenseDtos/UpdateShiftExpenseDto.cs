namespace DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos
{
    public class UpdateShiftExpenseDto
    {
        public Guid Id { get; set; }
        public Guid ShiftTruckEntryId { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
