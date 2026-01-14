namespace DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos
{
    public class CreateMaintenanceTypeDto
    {
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }
}
