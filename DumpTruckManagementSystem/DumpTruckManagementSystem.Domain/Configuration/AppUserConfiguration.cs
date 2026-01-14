using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            var admin = new AppUser
            {
                Id = GuidConstant.GenerateGuid1,                         // استخدم ثابت
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                SecurityStamp = GuidConstant.GenerateGuid2,              // مهم جداً
                ConcurrencyStamp = GuidConstant.GenerateGuid3            // مهم جداً
            };

            admin.PasswordHash = GeneratePasswordHash(admin, "Admin@0489");

            builder.HasData(admin);
        }

        private static string GeneratePasswordHash(AppUser user, string password)
        {
            var hasher = new PasswordHasher<AppUser>();
            return hasher.HashPassword(user, password);
        }
    }

}
