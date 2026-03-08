namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بيانات النشاط الأخير
    /// </summary>
    public class RecentActivityDto
    {
        /// <summary>
        /// نوع النشاط
        /// </summary>
        public string ActivityType { get; set; } = default!;

        /// <summary>
        /// عنوان النشاط
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// الوصف
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// تاريخ ووقت النشاط
        /// </summary>
        public DateTime ActivityDate { get; set; }
    }
}
