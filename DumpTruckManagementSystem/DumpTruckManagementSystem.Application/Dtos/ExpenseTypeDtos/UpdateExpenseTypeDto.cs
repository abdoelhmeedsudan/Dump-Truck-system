namespace DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos
{
    public class UpdateExpenseTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }
}
