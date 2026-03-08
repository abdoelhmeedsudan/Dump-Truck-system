namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بيانات أفضل القلابات
    /// </summary>
    public class TopTruckDto
    {
        /// <summary>
        /// معرف القلاب
        /// </summary>
        public Guid TruckId { get; set; }

        /// <summary>
        /// رقم القلاب
        /// </summary>
        public string TruckNumber { get; set; } = default!;

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
