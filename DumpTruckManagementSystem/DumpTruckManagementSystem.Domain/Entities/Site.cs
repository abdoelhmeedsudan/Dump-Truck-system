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
    // 3) Site (الموقع/المنجم)
    // =============================================================================================
    /// <summary>
    /// يمثل موقع التشغيل (منجم/موقع/منطقة).
    /// يسهّل الفلترة بالتاريخ والموقع وإعداد أسعار النقلات لكل موقع.
    /// </summary>
    public class Site : EntityBase<Guid>
    {
        /// <summary>
        /// اسم الموقع.
        /// مثال: "منجم 1"
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// رمز مختصر للموقع (اختياري).
        /// مثال: M1
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// ملاحظات خاصة بالموقع (اختياري).
        /// </summary>
        public string? Notes { get; set; }

        // -------------------------
        // العلاقات (Navigation)
        // -------------------------

        /// <summary>
        /// الورديات المسجلة في هذا الموقع.
        /// </summary>
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();

        /// <summary>
        /// أسعار النقلات (Rate per Trip) الخاصة بهذا الموقع عبر الزمن.
        /// </summary>
        public ICollection<RevenueRate> RevenueRates { get; set; } = new List<RevenueRate>();
    }
}
