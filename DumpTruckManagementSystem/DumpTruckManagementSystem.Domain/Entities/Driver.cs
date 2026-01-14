// ==================================================================================================
// DumpTruckManagementSystem - MVP Domain Entities (Single File)
// --------------------------------------------------------------------------------------------------
// الهدف: أقل نسخة MVP لنظام إدارة القلابات والورديات + المصاريف + الصيانة + أسعار النقلات.
// الملف يحتوي على:
// - Enums
// - Entities: DumpTruck, Driver, Site, Shift, ShiftTruckEntry, ExpenseType, ShiftExpense,
//            MaintenanceType, MaintenanceRecord, RevenueRate
// --------------------------------------------------------------------------------------------------
// ملاحظة 1: هذا الملف يفترض وجود EntityBase<Guid> داخل DumpTruckManagementSystem.Domain.Common.Base
//           ويحتوي على Id (وربما CreatedAt/UpdatedAt إذا كانت موجودة عندك).
// ملاحظة 2: استخدمت DateOnly للتواريخ (مناسب لـ .NET 6+). إذا لا ترغب، استبدله بـ DateTime.
// ==================================================================================================

using DumpTruckManagementSystem.Domain.Common.Base;

namespace DumpTruckManagementSystem.Domain.Entities
{
    // =============================================================================================
    // 2) Driver (السائق)
    // =============================================================================================
    /// <summary>
    /// يمثل السائق (Master Data).
    /// يُربط بالورديات اليومية لمعرفة من قاد القلاب خلال اليوم.
    /// </summary>
    public class Driver : EntityBase<Guid>
    {
        /// <summary>
        /// الاسم الكامل للسائق.
        /// </summary>
        public string FullName { get; set; } = default!;

        /// <summary>
        /// رقم الجوال (اختياري).
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// رقم الهوية/الإقامة (اختياري).
        /// </summary>
        public string? NationalId { get; set; }

        /// <summary>
        /// حالة السائق (نشط/غير نشط).
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ملاحظات عن السائق.
        /// </summary>
        public string? Notes { get; set; }

        // -------------------------
        // العلاقات (Navigation)
        // -------------------------

        /// <summary>
        /// إدخالات السائق في الورديات (أي الأيام التي عمل بها).
        /// </summary>
        public ICollection<ShiftTruckEntry> ShiftEntries { get; set; } = new List<ShiftTruckEntry>();
    }
}
