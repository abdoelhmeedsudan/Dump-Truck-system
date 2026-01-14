using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = GuidConstant.GenerateGuid1,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = GuidConstant.GenerateGuid2,
                    Name = "SystemManager",
                    NormalizedName = "SYSTEMMANAGER"
                },
                new IdentityRole
                {
                    Id = GuidConstant.GenerateGuid3,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = GuidConstant.GenerateGuid4,
                    Name = "Driver",
                    NormalizedName = "DRIVER"
                }
            );
        }
    }

}
