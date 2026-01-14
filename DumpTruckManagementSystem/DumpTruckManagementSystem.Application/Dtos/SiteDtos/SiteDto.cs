namespace DumpTruckManagementSystem.Application.Dtos.SiteDtos
{
    public class SiteDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Code { get; set; }
        public string? Notes { get; set; }
    }
}
