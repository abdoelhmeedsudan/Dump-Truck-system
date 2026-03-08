namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بيانات لوحة التحكم الرئيسية
    /// </summary>
    public class DashboardDto
    {
        /// <summary>
        /// بطاقات الإحصائيات الرئيسية
        /// </summary>
        public DashboardStatsDto Stats { get; set; } = new();

        /// <summary>
        /// بيانات الرسم البياني للإيرادات والمصروفات
        /// </summary>
        public List<RevenueExpenseChartDto> RevenueExpenseChart { get; set; } = new();

        /// <summary>
        /// بيانات حالة الأسطول
        /// </summary>
        public FleetStatusDto FleetStatus { get; set; } = new();

        /// <summary>
        /// النشاط الأخير
        /// </summary>
        public List<RecentActivityDto> RecentActivities { get; set; } = new();

        /// <summary>
        /// بيانات الرسم البياني للنقلات الشهرية
        /// </summary>
        public List<TripsChartDto> TripsChart { get; set; } = new();

        /// <summary>
        /// أفضل السائقين (أعلى 5)
        /// </summary>
        public List<TopDriverDto> TopDrivers { get; set; } = new();

        /// <summary>
        /// أفضل القلابات (أعلى 5)
        /// </summary>
        public List<TopTruckDto> TopTrucks { get; set; } = new();

        /// <summary>
        /// إحصائيات المواقع
        /// </summary>
        public List<SiteStatisticsDto> SiteStatistics { get; set; } = new();
    }
}
