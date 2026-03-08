using System.ComponentModel.DataAnnotations;

namespace DumpTruckManagementSystem.Application.Dtos.AuthDtos
{
    /// <summary>
    /// بيانات تسجيل الدخول
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// اسم المستخدم أو البريد الإلكتروني
        /// </summary>
        [Required(ErrorMessage = "اسم المستخدم أو البريد الإلكتروني مطلوب")]
        public string UserNameOrEmail { get; set; } = default!;

        /// <summary>
        /// كلمة المرور
        /// </summary>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        public string Password { get; set; } = default!;
    }
}
