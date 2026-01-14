using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class ShiftTruckEntryConfiguration : IEntityTypeConfiguration<ShiftTruckEntry>
    {
        public void Configure(EntityTypeBuilder<ShiftTruckEntry> builder)
        {
            builder.ToTable("ShiftTruckEntries");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();


            builder.Property(e => e.Notes)
                   .HasMaxLength(50);


            builder.Property(e => e.TripUnitPrice)
                   .HasColumnType("decimal(10,2)");


            builder.HasOne(x => x.Shift).WithMany(s => s.TruckEntries).HasForeignKey(f => f.ShiftId);

            builder.HasOne(x => x.DumpTruck).WithMany(s => s.ShiftEntries).HasForeignKey(f => f.DumpTruckId);

            builder.HasOne(x => x.Driver).WithMany(s => s.ShiftEntries).HasForeignKey(f => f.DriverId);

            builder.HasMany(x => x.Expenses).WithOne(s => s.ShiftTruckEntry).HasForeignKey(f => f.ShiftTruckEntryId);

        }
    }

}
