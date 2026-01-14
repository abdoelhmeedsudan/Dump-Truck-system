namespace DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos
{
    public class UpdateShiftTruckEntryDto
    {
        public Guid Id { get; set; }
        public Guid ShiftId { get; set; }
        public Guid DumpTruckId { get; set; }
        public Guid? DriverId { get; set; }
        public int TripsCount { get; set; }
        public decimal? TripUnitPrice { get; set; }
        public string? Notes { get; set; }
    }
}
