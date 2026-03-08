namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بيانات الرسم البياني للإيرادات والمصروفات
    /// </summary>
    public class RevenueExpenseChartDto
    {
        /// <summary>
        /// الشهر (مثال: "يناير 2024")
        /// </summary>
        public string Month { get; set; } = default!;

        /// <summary>
        /// الإيرادات الشهرية
        /// </summary>
        public decimal Revenue { get; set; }

        /// <summary>
        /// المصروفات الشهرية
        /// </summary>
        public decimal Expenses { get; set; }
    }
}
