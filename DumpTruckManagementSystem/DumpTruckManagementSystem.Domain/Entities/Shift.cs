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
    // 4) Shift (رأس الوردية اليومية)
    // =============================================================================================
    /// <summary>
    /// يمثل يوم التشغيل في موقع معين.
    /// عادة يكون هناك Shift واحد لكل (تاريخ + موقع) لتسهيل المتابعة اليومية.
    /// </summary>
    public class Shift : EntityBase<Guid>
    {
        /// <summary>
        /// تاريخ الوردية (اليوم).
        /// </summary>
        public DateOnly ShiftDate { get; set; }

        /// <summary>
        /// الموقع الذي تم التشغيل فيه في هذا اليوم.
        /// </summary>
        public Guid SiteId { get; set; }

        /// <summary>
        /// كيان الموقع المرتبط.
        /// </summary>
        public Site Site { get; set; } = default!;

        /// <summary>
        /// ملاحظات عامة عن اليوم (اختياري).
        /// </summary>
        public string? Notes { get; set; }

        // -------------------------
        // العلاقات (Navigation)
        // -------------------------

        /// <summary>
        /// تفاصيل القلابات التي عملت في هذه الوردية (سطر لكل قلاب).
        /// </summary>
        public ICollection<ShiftTruckEntry> TruckEntries { get; set; } = new List<ShiftTruckEntry>();
    }
}
