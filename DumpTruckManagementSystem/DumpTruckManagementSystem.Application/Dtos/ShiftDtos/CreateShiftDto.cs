namespace DumpTruckManagementSystem.Application.Dtos.ShiftDtos
{
    public class CreateShiftDto
    {
        public DateOnly ShiftDate { get; set; }
        public Guid SiteId { get; set; }
        public string? Notes { get; set; }
    }
}
