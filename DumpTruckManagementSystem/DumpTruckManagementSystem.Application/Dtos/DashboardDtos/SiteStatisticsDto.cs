namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// إحصائيات الموقع
    /// </summary>
    public class SiteStatisticsDto
    {
        /// <summary>
        /// معرف الموقع
        /// </summary>
        public Guid SiteId { get; set; }

        /// <summary>
        /// اسم الموقع
        /// </summary>
        public string SiteName { get; set; } = default!;

        /// <summary>
        /// عدد النقلات الشهرية
        /// </summary>
        public int MonthlyTrips { get; set; }

        /// <summary>
        /// الإيرادات الشهرية
        /// </summary>
        public decimal MonthlyRevenue { get; set; }

        /// <summary>
        /// عدد الورديات الشهرية
        /// </summary>
        public int MonthlyShifts { get; set; }
    }
}
