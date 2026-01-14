namespace DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos
{
    public class CreateExpenseTypeDto
    {
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }
}
