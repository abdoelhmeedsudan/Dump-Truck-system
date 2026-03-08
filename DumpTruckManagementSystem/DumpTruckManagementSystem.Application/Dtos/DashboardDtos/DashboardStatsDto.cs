namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بطاقات الإحصائيات الرئيسية للوحة التحكم
    /// </summary>
    public class DashboardStatsDto
    {
        /// <summary>
        /// إجمالي الشاحنات (القلابات)
        /// </summary>
        public int TotalTrucks { get; set; }

        /// <summary>
        /// السائقين النشطين
        /// </summary>
        public int ActiveDrivers { get; set; }

        /// <summary>
        /// الإيرادات الشهرية
        /// </summary>
        public decimal MonthlyRevenue { get; set; }

        /// <summary>
        /// صافي الربح
        /// </summary>
        public decimal NetProfit { get; set; }

        /// <summary>
        /// إجمالي النقلات الشهرية
        /// </summary>
        public int MonthlyTrips { get; set; }

        /// <summary>
        /// إجمالي المصروفات الشهرية
        /// </summary>
        public decimal MonthlyExpenses { get; set; }

        /// <summary>
        /// متوسط النقلات يومياً (الشهر الحالي)
        /// </summary>
        public double AverageDailyTrips { get; set; }

        /// <summary>
        /// عدد الورديات الشهرية
        /// </summary>
        public int MonthlyShifts { get; set; }
    }
}
