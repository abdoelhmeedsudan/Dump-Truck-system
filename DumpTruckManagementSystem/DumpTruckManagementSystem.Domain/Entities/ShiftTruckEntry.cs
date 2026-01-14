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
    // 5) ShiftTruckEntry (تفاصيل القلاب داخل الوردية)
    // =============================================================================================
    /// <summary>
    /// يمثل سطر القلاب في يوم محدد:
    /// - القلاب
    /// - السائق (اختياري)
    /// - عدد النقلات
    /// - ملاحظات
    /// + مصاريف مرتبطة بهذا السطر
    /// </summary>
    public class ShiftTruckEntry : EntityBase<Guid>
    {
        /// <summary>
        /// مرجع الوردية (اليوم/الموقع).
        /// </summary>
        public Guid ShiftId { get; set; }

        /// <summary>
        /// كيان الوردية المرتبط.
        /// </summary>
        public Shift Shift { get; set; } = default!;

        /// <summary>
        /// مرجع القلاب الذي تم تشغيله.
        /// </summary>
        public Guid DumpTruckId { get; set; }

        /// <summary>
        /// كيان القلاب المرتبط.
        /// </summary>
        public DumpTruck DumpTruck { get; set; } = default!;

        /// <summary>
        /// مرجع السائق (اختياري - قد يكون غير محدد).
        /// </summary>
        public Guid? DriverId { get; set; }

        /// <summary>
        /// كيان السائق المرتبط (اختياري).
        /// </summary>
        public Driver? Driver { get; set; }

        /// <summary>
        /// عدد النقلات المنجزة في هذا اليوم لهذا القلاب.
        /// </summary>
        public int TripsCount { get; set; }

        /// <summary>
        /// سعر النقلة (اختياري).
        /// إذا رغبت بتثبيت السعر وقت الإدخال بدل الرجوع لجدول RevenueRates.
        /// </summary>
        public decimal? TripUnitPrice { get; set; }

        /// <summary>
        /// ملاحظات تشغيلية لهذا القلاب في هذا اليوم.
        /// </summary>
        public string? Notes { get; set; }

        // -------------------------
        // العلاقات (Navigation)
        // -------------------------

        /// <summary>
        /// المصاريف التي حدثت لهذا القلاب في هذا اليوم (جاز/بنشر/قطع...).
        /// </summary>
        public ICollection<ShiftExpense> Expenses { get; set; } = new List<ShiftExpense>();
    }
}
