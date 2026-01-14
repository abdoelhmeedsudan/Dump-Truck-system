using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class RevenueRateConfiguration : IEntityTypeConfiguration<RevenueRate>
    {
        public void Configure(EntityTypeBuilder<RevenueRate> builder)
        {
            builder.ToTable("RevenueRates");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.EffectiveFrom)
                   .IsRequired();

            builder.Property(e => e.RatePerTrip)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.Property(e => e.CurrencyCode)
                   .IsRequired()
                   .HasMaxLength(10)
                   .HasDefaultValue("SAR");

            builder.Property(e => e.ExchangeRateToSar)
                   .HasColumnType("decimal(10,4)");

            builder.Property(e => e.Notes)
                   .HasMaxLength(500);

            builder.HasOne(x => x.Site)
                   .WithMany(x => x.RevenueRates)
                   .HasForeignKey(x => x.SiteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
