namespace DumpTruckManagementSystem.Application.Dtos.AuthDtos
{
    /// <summary>
    /// استجابة المصادقة (Login/Register)
    /// </summary>
    public class AuthResponseDto
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        public string Token { get; set; } = default!;

        /// <summary>
        /// تاريخ انتهاء الصلاحية
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// معرف المستخدم
        /// </summary>
        public string UserId { get; set; } = default!;

        /// <summary>
        /// اسم المستخدم
        /// </summary>
        public string UserName { get; set; } = default!;

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// الاسم الكامل
        /// </summary>
        public string FullName { get; set; } = default!;
    }
}
