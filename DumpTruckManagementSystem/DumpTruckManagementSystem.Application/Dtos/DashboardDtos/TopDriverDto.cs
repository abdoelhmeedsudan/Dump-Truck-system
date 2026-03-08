namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بيانات أفضل السائقين
    /// </summary>
    public class TopDriverDto
    {
        /// <summary>
        /// معرف السائق
        /// </summary>
        public Guid DriverId { get; set; }

        /// <summary>
        /// اسم السائق
        /// </summary>
        public string DriverName { get; set; } = default!;

        /// <summary>
        /// إجمالي النقلات
        /// </summary>
        public int TotalTrips { get; set; }

        /// <summary>
        /// إجمالي الإيرادات
        /// </summary>
        public decimal TotalRevenue { get; set; }
    }
}
