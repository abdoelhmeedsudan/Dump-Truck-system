namespace DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos
{
    public class ShiftExpenseDto
    {
        public Guid Id { get; set; }
        public Guid ShiftTruckEntryId { get; set; }
        public Guid ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
