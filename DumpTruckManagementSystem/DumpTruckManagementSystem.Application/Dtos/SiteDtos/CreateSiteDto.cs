namespace DumpTruckManagementSystem.Application.Dtos.SiteDtos
{
    public class CreateSiteDto
    {
        public string Name { get; set; } = default!;
        public string? Code { get; set; }
        public string? Notes { get; set; }
    }
}
