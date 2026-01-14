using DumpTruckManagementSystem.Domain.Entities;
using DumpTruckManagementSystem.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DumpTruckManagementSystem.Domain.Configuration
{
    public class ShiftExpenseConfiguration : IEntityTypeConfiguration<ShiftExpense>
    {
        public void Configure(EntityTypeBuilder<ShiftExpense> builder)
        {
            builder.ToTable("ShiftExpenses");

            builder.HasKey(x => x.Id);

            builder.Property(e => e.Id)
                   .HasDefaultValueSql(GuidConstant.SequentialGuid)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(10,2)");

            builder.Property(e => e.Notes)
                   .HasMaxLength(500);

            builder.HasOne(x => x.ShiftTruckEntry)
                   .WithMany(x => x.Expenses)
                   .HasForeignKey(x => x.ShiftTruckEntryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.ExpenseType)
                   .WithMany(x => x.ShiftExpenses)
                   .HasForeignKey(x => x.ExpenseTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
