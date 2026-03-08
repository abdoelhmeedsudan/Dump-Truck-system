namespace DumpTruckManagementSystem.Application.Dtos.DashboardDtos
{
    /// <summary>
    /// بيانات حالة الأسطول للرسم الدائري
    /// </summary>
    public class FleetStatusDto
    {
        /// <summary>
        /// عدد القلابات النشطة
        /// </summary>
        public int ActiveTrucks { get; set; }

        /// <summary>
        /// عدد القلابات في الصيانة
        /// </summary>
        public int MaintenanceTrucks { get; set; }

        /// <summary>
        /// عدد القلابات المتوقفة
        /// </summary>
        public int InactiveTrucks { get; set; }
    }
}
