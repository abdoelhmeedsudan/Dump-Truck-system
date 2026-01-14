using DumpTruckManagementSystem.Domain.Enums;

namespace DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos
{
    public class DumpTruckDto
    {
        public Guid Id { get; set; }
        public string TruckNumber { get; set; } = default!;
        public string PlateNumber { get; set; } = default!;
        public string TruckType { get; set; } = default!;
        public string? Model { get; set; }
        public decimal LoadCapacity { get; set; }
        public DumpTruckStatus Status { get; set; }
        public string? Notes { get; set; }

    }
}
