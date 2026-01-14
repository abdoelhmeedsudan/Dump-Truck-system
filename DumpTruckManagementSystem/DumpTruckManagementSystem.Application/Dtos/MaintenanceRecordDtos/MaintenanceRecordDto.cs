namespace DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos
{
    public class MaintenanceRecordDto
    {
        public Guid Id { get; set; }
        public DateOnly MaintenanceDate { get; set; }
        public Guid DumpTruckId { get; set; }
        public Guid MaintenanceTypeId { get; set; }
        public decimal PartsCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal TotalCost { get; set; }
        public string? DoneBy { get; set; }
        public string? Notes { get; set; }
    }
}
