namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بيانات الرسم البياني للنقلات الشهرية
    /// </summary>
    public class TripsChartDto
    {
        /// <summary>
        /// الشهر (مثال: "يناير 2024")
        /// </summary>
        public string Month { get; set; } = default!;

        /// <summary>
        /// عدد النقلات
        /// </summary>
        public int TripsCount { get; set; }
    }
}
