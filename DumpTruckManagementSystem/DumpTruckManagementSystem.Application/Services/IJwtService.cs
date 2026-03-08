using DumpTruckManagementSystem.Domain.Entities;

namespace DumpTruckManagementSystem.Application.Services
{
    /// <summary>
    /// واجهة خدمة JWT لتوليد Token
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// توليد JWT Token للمستخدم
        /// </summary>
        string GenerateToken(AppUser user, IList<string> roles);
    }
}
