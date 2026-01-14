namespace DumpTruckManagementSystem.Application.Dtos.DriverDtos
{
    public class UpdateDriverDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = default!;
        public string? PhoneNumber { get; set; }
        public string? NationalId { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
    }
}
