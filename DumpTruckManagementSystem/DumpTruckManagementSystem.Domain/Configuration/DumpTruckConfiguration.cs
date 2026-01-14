using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class DumpTruckConfiguration : IEntityTypeConfiguration<DumpTruck>
    {
        public void Configure(EntityTypeBuilder<DumpTruck> builder)
        {
            builder.ToTable("DumpTrucks");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.TruckNumber)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(e => e.TruckType)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Model)
                   .HasMaxLength(50);

            builder.Property(e => e.PlateNumber)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.Property(e => e.LoadCapacity)
                   .HasColumnType("decimal(10,2)");


        }
    }

}
