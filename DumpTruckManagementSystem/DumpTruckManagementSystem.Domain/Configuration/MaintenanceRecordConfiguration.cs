using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class MaintenanceRecordConfiguration : IEntityTypeConfiguration<MaintenanceRecord>
    {
        public void Configure(EntityTypeBuilder<MaintenanceRecord> builder)
        {
            builder.ToTable("MaintenanceRecords");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.MaintenanceDate)
                   .IsRequired();

            builder.Property(e => e.PartsCost)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.Property(e => e.LaborCost)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.Property(e => e.TotalCost)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.Property(e => e.DoneBy)
                   .HasMaxLength(200);

            builder.Property(e => e.Notes)
                   .HasMaxLength(500);

            builder.HasOne(x => x.DumpTruck)
                   .WithMany(x => x.MaintenanceRecords)
                   .HasForeignKey(x => x.DumpTruckId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MaintenanceType)
                   .WithMany(x => x.MaintenanceRecords)
                   .HasForeignKey(x => x.MaintenanceTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
