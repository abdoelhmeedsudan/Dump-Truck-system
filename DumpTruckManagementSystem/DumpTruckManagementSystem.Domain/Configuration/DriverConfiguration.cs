using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class DriverConfiguration : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Drivers");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.NationalId)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.PhoneNumber)
                   .HasMaxLength(50);

            builder.Property(e => e.PhoneNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasMany(x => x.ShiftEntries).WithOne(o => o.Driver).HasForeignKey(f => f.DriverId);

        }
    }

}
