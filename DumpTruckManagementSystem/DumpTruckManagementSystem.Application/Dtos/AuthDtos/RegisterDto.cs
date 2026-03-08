using System.ComponentModel.DataAnnotations;

namespace DumpTruckManagementSystem.Application.Dtos.AuthDtos
{
    /// <summary>
    /// بيانات إنشاء مستخدم جديد
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// الاسم الكامل
        /// </summary>
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        public string FullName { get; set; } = default!;

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string UserName { get; set; } = default!;

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        public string Email { get; set; } = default!;

        /// <summary>
        /// كلمة المرور
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 6 أحرف")]
        public string Password { get; set; } = default!;

        /// <summary>
        /// تأكيد كلمة المرور
        /// </summary>
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; } = default!;
    }
}
