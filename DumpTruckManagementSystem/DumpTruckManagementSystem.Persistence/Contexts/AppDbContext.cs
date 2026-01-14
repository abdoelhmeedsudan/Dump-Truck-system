using DumpTruckManagementSystem.Domain.Configuration;
using DumpTruckManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Persistence.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public virtual DbSet<DumpTruck> DumpTrucks { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<ShiftTruckEntry> ShiftTruckEntries { get; set; }
        public virtual DbSet<ExpenseType> ExpenseTypes { get; set; }
        public virtual DbSet<ShiftExpense> ShiftExpenses { get; set; }
        public virtual DbSet<MaintenanceType> MaintenanceTypes { get; set; }
        public virtual DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
        public virtual DbSet<RevenueRate> RevenueRates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new DumpTruckConfiguration());
            builder.ApplyConfiguration(new AppUserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new DriverConfiguration());
            builder.ApplyConfiguration(new SiteConfiguration());
            builder.ApplyConfiguration(new ShiftConfiguration());
            builder.ApplyConfiguration(new ShiftTruckEntryConfiguration());
            builder.ApplyConfiguration(new ExpenseTypeConfiguration());
            builder.ApplyConfiguration(new ShiftExpenseConfiguration());
            builder.ApplyConfiguration(new MaintenanceTypeConfiguration());
            builder.ApplyConfiguration(new MaintenanceRecordConfiguration());
            builder.ApplyConfiguration(new RevenueRateConfiguration());
            base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
