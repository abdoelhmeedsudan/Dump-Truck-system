namespace DumpTruckManagementSystem.Application.Dtos.ShiftDtos
{
    public class UpdateShiftDto
    {
        public Guid Id { get; set; }
        public DateOnly ShiftDate { get; set; }
        public Guid SiteId { get; set; }
        public string? Notes { get; set; }
    }
}
