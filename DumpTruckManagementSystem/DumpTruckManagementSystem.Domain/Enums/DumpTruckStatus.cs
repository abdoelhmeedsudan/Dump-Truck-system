using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumpTruckManagementSystem.Domain.Enums
{
    /// <summary>
    /// حالة القلاب التشغيلية (بديل أفضل من bool IsActive لأنه يغطي أكثر من حالة).
    /// </summary>
    public enum DumpTruckStatus
    {
        /// <summary>شغال / متاح للعمل</summary>
        Active = 1,

        /// <summary>غير نشط / متوقف</summary>
        Inactive = 2,

        /// <summary>تحت الصيانة / غير متاح للتشغيل</summary>
        UnderMaintenance = 3
    }
}
