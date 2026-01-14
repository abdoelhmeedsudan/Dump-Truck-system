using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.ToTable("Shifts");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.Notes)
                   .IsRequired()
                   .HasMaxLength(50);
            builder.HasOne(x => x.Site).WithMany(x => x.Shifts).HasForeignKey(x => x.SiteId);
            builder.HasMany(x => x.TruckEntries).WithOne(x => x.Shift).HasForeignKey(x => x.ShiftId);

        }
    }

}
