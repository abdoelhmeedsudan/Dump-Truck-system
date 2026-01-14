using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class SiteConfiguration : IEntityTypeConfiguration<Site>
    {
        public void Configure(EntityTypeBuilder<Site> builder)
        {
            builder.ToTable("Sites");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.Code)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.Notes)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Name)
                   .HasMaxLength(50);

         
        }
    }

}
