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
using DumpTruckManagementSystem.Domain.Enums;

namespace DumpTruckManagementSystem.Domain.Entities
{



    // =============================================================================================
    // 1) DumpTruck (القلاب)
    // =============================================================================================
    /// <summary>
    /// يمثل القلاب داخل الأسطول (Master Data).
    /// يُستخدم في الورديات اليومية والصيانة والتقارير.
    /// </summary>
    public class DumpTruck : EntityBase<Guid>
    {
        /// <summary>
        /// رقم القلاب الداخلي (معرّف تشغيلي فريد داخل الشركة).
        /// مثال: DT-001
        /// </summary>
        public string TruckNumber { get; set; } = default!;

        /// <summary>
        /// رقم اللوحة (مفيد للتمييز الرسمي).
        /// </summary>
        public string PlateNumber { get; set; } = default!;

        /// <summary>
        /// نوع القلاب (MVP كـ string).
        /// لاحقًا يمكن تحويله إلى Enum أو Lookup Table لتفادي أخطاء الإدخال.
        /// مثال: "Small" / "Medium" / "Large"
        /// </summary>
        public string TruckType { get; set; } = default!;

        /// <summary>
        /// موديل القلاب (اختياري).
        /// </summary>
        public string? Model { get; set; }

        /// <summary>
        /// الحمولة القصوى (بالطن أو حسب معيارك).
        /// </summary>
        public decimal LoadCapacity { get; set; }

        /// <summary>
        /// الحالة التشغيلية للقلاب (شغال/متوقف/تحت الصيانة).
        /// </summary>
        public DumpTruckStatus Status { get; set; } = DumpTruckStatus.Active;

        /// <summary>
        /// ملاحظات عامة عن القلاب.
        /// </summary>
        public string? Notes { get; set; }

        // -------------------------
        // العلاقات (Navigation)
        // -------------------------

        /// <summary>
        /// كل إدخالات هذا القلاب في الورديات (اليومية).
        /// </summary>
        public ICollection<ShiftTruckEntry> ShiftEntries { get; set; } = new List<ShiftTruckEntry>();

        /// <summary>
        /// سجلات الصيانة الخاصة بهذا القلاب.
        /// </summary>
        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
    }

    // =============================================================================================
    // 6) ExpenseType (نوع المصروف)
    // =============================================================================================
    /// <summary>
    /// أنواع المصاريف التشغيلية (Master Data).
    /// مثال: جاز، بنشر، قطع غيار...
    /// </summary>
    public class ExpenseType : EntityBase<Guid>
    {
        /// <summary>
        /// اسم نوع المصروف.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// هل هذا النوع متاح للاستخدام أم لا.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ملاحظات (اختياري).
        /// </summary>
        public string? Notes { get; set; }

        // -------------------------
        // العلاقات (Navigation)
        // -------------------------

        /// <summary>
        /// المصاريف اليومية المرتبطة بهذا النوع.
        /// </summary>
        public ICollection<ShiftExpense> ShiftExpenses { get; set; } = new List<ShiftExpense>();
    }

    // =============================================================================================
    // 7) ShiftExpense (مصروف يومي داخل الوردية)
    // =============================================================================================
    /// <summary>
    /// يمثل مصروفًا مرتبطًا بسطر تشغيل (قلاب + يوم).
    /// </summary>
    public class ShiftExpense : EntityBase<Guid>
    {
        /// <summary>
        /// مرجع سطر القلاب في الوردية.
        /// </summary>
        public Guid ShiftTruckEntryId { get; set; }

        /// <summary>
        /// كيان سطر القلاب المرتبط.
        /// </summary>
        public ShiftTruckEntry ShiftTruckEntry { get; set; } = default!;

        /// <summary>
        /// نوع المصروف (جاز/بنشر...).
        /// </summary>
        public Guid ExpenseTypeId { get; set; }

        /// <summary>
        /// كيان نوع المصروف المرتبط.
        /// </summary>
        public ExpenseType ExpenseType { get; set; } = default!;

        /// <summary>
        /// قيمة المصروف.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// ملاحظات/وصف (اختياري).
        /// مثال: "جاز - محطة X"
        /// </summary>
        public string? Notes { get; set; }
    }

    // =============================================================================================
    // 8) MaintenanceType (نوع الصيانة)
    // =============================================================================================
    /// <summary>
    /// أنواع الصيانة (Master Data).
    /// مثال: تغيير زيت، كهرباء، دبرياج...
    /// </summary>
    public class MaintenanceType : EntityBase<Guid>
    {
        /// <summary>
        /// اسم نوع الصيانة.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// هل النوع متاح للاستخدام.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// ملاحظات (اختياري).
        /// </summary>
        public string? Notes { get; set; }

        // -------------------------
        // العلاقات (Navigation)
        // -------------------------

        /// <summary>
        /// سجلات الصيانة المرتبطة بهذا النوع.
        /// </summary>
        public ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
    }

    // =============================================================================================
    // 9) MaintenanceRecord (سجل صيانة)
    // =============================================================================================
    /// <summary>
    /// يمثل عملية صيانة واحدة للقلاب.
    /// تُستخدم لتجميع مصاريف الصيانة ضمن التقارير الشهرية.
    /// </summary>
    public class MaintenanceRecord : EntityBase<Guid>
    {
        /// <summary>
        /// تاريخ الصيانة.
        /// </summary>
        public DateOnly MaintenanceDate { get; set; }

        /// <summary>
        /// القلاب الذي تمت صيانته.
        /// </summary>
        public Guid DumpTruckId { get; set; }

        /// <summary>
        /// كيان القلاب المرتبط.
        /// </summary>
        public DumpTruck DumpTruck { get; set; } = default!;

        /// <summary>
        /// نوع الصيانة (تغيير زيت/كهرباء...).
        /// </summary>
        public Guid MaintenanceTypeId { get; set; }

        /// <summary>
        /// كيان نوع الصيانة المرتبط.
        /// </summary>
        public MaintenanceType MaintenanceType { get; set; } = default!;

        /// <summary>
        /// تكلفة قطع الغيار.
        /// </summary>
        public decimal PartsCost { get; set; }

        /// <summary>
        /// تكلفة العمل/الأجرة.
        /// </summary>
        public decimal LaborCost { get; set; }

        /// <summary>
        /// إجمالي التكلفة.
        /// ملاحظة: يمكن جعلها محسوبة بدل تخزينها:
        /// TotalCost = PartsCost + LaborCost
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// اسم الفني/الورشة التي نفذت الصيانة (اختياري).
        /// </summary>
        public string? DoneBy { get; set; }

        /// <summary>
        /// ملاحظات إضافية (اختياري).
        /// </summary>
        public string? Notes { get; set; }
    }

    // =============================================================================================
    // 10) RevenueRate (سعر النقلة)
    // =============================================================================================
    /// <summary>
    /// سعر النقلة حسب الموقع عبر الزمن.
    /// الهدف: دعم اختلاف سعر النقلة من شهر لآخر أو من موقع لآخر.
    /// </summary>
    public class RevenueRate : EntityBase<Guid>
    {
        /// <summary>
        /// الموقع الذي يطبق عليه هذا السعر.
        /// </summary>
        public Guid SiteId { get; set; }

        /// <summary>
        /// كيان الموقع المرتبط.
        /// </summary>
        public Site Site { get; set; } = default!;

        /// <summary>
        /// تاريخ بداية تطبيق السعر.
        /// </summary>
        public DateOnly EffectiveFrom { get; set; }

        /// <summary>
        /// قيمة النقلة الواحدة.
        /// </summary>
        public decimal RatePerTrip { get; set; }

        /// <summary>
        /// رمز العملة (MVP كـ نص).
        /// مثال: SAR, USD
        /// </summary>
        public string CurrencyCode { get; set; } = "SAR";

        /// <summary>
        /// سعر الصرف إلى SAR (اختياري).
        /// لو كانت العملة غير SAR وتحتاج تحويل في التقرير الشهري.
        /// </summary>
        public decimal? ExchangeRateToSar { get; set; }

        /// <summary>
        /// ملاحظات (اختياري).
        /// مثال: "تحديث أسعار يناير"
        /// </summary>
        public string? Notes { get; set; }
    }
}
