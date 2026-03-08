using DumpTruckManagementSystem.Application.Dtos.DashboardDtos;
using DumpTruckManagementSystem.Application.Features.DashboardFeature.Queries.Query;
using DumpTruckManagementSystem.Domain.Enums;
using DumpTruckManagementSystem.Persistence.Contexts;
using DumpTruckManagementSystem.Shared.Wrappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DumpTruckManagementSystem.Application.Features.DashboardFeature.Queries.Handler
{
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, Response<DashboardDto>>
    {
        private readonly AppDbContext _context;

        public GetDashboardQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Response<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var dashboard = new DashboardDto();

            // جلب البيانات بشكل متسلسل لتجنب مشاكل DbContext threading
            // (DbContext ليس thread-safe ولا يمكن استخدامه بشكل متوازي)
            dashboard.Stats = await CalculateStatsAsync(cancellationToken);
            dashboard.FleetStatus = await CalculateFleetStatusAsync(cancellationToken);
            dashboard.RevenueExpenseChart = await CalculateRevenueExpenseChartAsync(cancellationToken);
            dashboard.TripsChart = await CalculateTripsChartAsync(cancellationToken);
            dashboard.TopDrivers = await GetTopDriversAsync(cancellationToken);
            dashboard.TopTrucks = await GetTopTrucksAsync(cancellationToken);
            dashboard.SiteStatistics = await GetSiteStatisticsAsync(cancellationToken);
            dashboard.RecentActivities = await GetRecentActivitiesAsync(cancellationToken);

            return new Response<DashboardDto>
            {
                Data = dashboard,
                Succeeded = true,
                HttpStatusCode = System.Net.HttpStatusCode.OK
            };
        }

        private async Task<DashboardStatsDto> CalculateStatsAsync(CancellationToken cancellationToken)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // جلب الإحصائيات الأساسية بشكل متسلسل (DbContext ليس thread-safe)
            var totalTrucks = await _context.DumpTrucks
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .CountAsync(cancellationToken);

            var activeDrivers = await _context.Drivers
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive)
                .CountAsync(cancellationToken);

            // الإيرادات الشهرية (من ShiftTruckEntry)
            var monthlyEntries = await _context.ShiftTruckEntries
                .AsNoTracking()
                .Include(x => x.Shift)
                .Where(x => !x.IsDeleted &&
                           x.Shift.ShiftDate.Year == currentYear &&
                           x.Shift.ShiftDate.Month == currentMonth)
                .ToListAsync(cancellationToken);

            // المصروفات الشهرية (ShiftExpense + MaintenanceRecord)
            var monthlyExpenses = await _context.ShiftExpenses
                .AsNoTracking()
                .Include(x => x.ShiftTruckEntry)
                .ThenInclude(x => x.Shift)
                .Where(x => !x.IsDeleted &&
                           x.ShiftTruckEntry.Shift.ShiftDate.Year == currentYear &&
                           x.ShiftTruckEntry.Shift.ShiftDate.Month == currentMonth)
                .SumAsync(x => x.Amount, cancellationToken);

            var monthlyMaintenanceExpenses = await _context.MaintenanceRecords
                .AsNoTracking()
                .Where(x => !x.IsDeleted &&
                           x.MaintenanceDate.Year == currentYear &&
                           x.MaintenanceDate.Month == currentMonth)
                .SumAsync(x => x.TotalCost, cancellationToken);

            var monthlyShifts = await _context.Shifts
                .AsNoTracking()
                .Where(x => !x.IsDeleted &&
                           x.ShiftDate.Year == currentYear &&
                           x.ShiftDate.Month == currentMonth)
                .CountAsync(cancellationToken);

            var monthlyRevenue = monthlyEntries.Sum(x =>
            {
                if (x.TripUnitPrice.HasValue)
                    return x.TripsCount * x.TripUnitPrice.Value;
                else
                    return x.TripsCount * GetRevenueRateForShift(x.Shift.SiteId, x.Shift.ShiftDate);
            });

            var totalMonthlyExpenses = monthlyExpenses + monthlyMaintenanceExpenses;

            // صافي الربح
            var netProfit = monthlyRevenue - totalMonthlyExpenses;

            // إجمالي النقلات الشهرية
            var monthlyTrips = monthlyEntries.Sum(x => x.TripsCount);

            // متوسط النقلات يومياً
            var daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
            var averageDailyTrips = monthlyShifts > 0 ? (double)monthlyTrips / monthlyShifts : 0;

            return new DashboardStatsDto
            {
                TotalTrucks = totalTrucks,
                ActiveDrivers = activeDrivers,
                MonthlyRevenue = monthlyRevenue,
                NetProfit = netProfit,
                MonthlyTrips = monthlyTrips,
                MonthlyExpenses = totalMonthlyExpenses,
                AverageDailyTrips = Math.Round(averageDailyTrips, 2),
                MonthlyShifts = monthlyShifts
            };
        }

        private async Task<List<RevenueExpenseChartDto>> CalculateRevenueExpenseChartAsync(CancellationToken cancellationToken)
        {
            var currentDate = DateTime.Now;
            var months = new[] { "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" };

            // حساب نطاق التواريخ (آخر 12 شهر)
            var startDate = currentDate.AddMonths(-11).Date;
            var endDate = currentDate.Date;

            // جلب جميع البيانات دفعة واحدة بدلاً من loop
            var allEntries = await _context.ShiftTruckEntries
                .AsNoTracking()
                .Include(x => x.Shift)
                .Where(x => !x.IsDeleted &&
                           x.Shift.ShiftDate >= DateOnly.FromDateTime(startDate) &&
                           x.Shift.ShiftDate <= DateOnly.FromDateTime(endDate))
                .ToListAsync(cancellationToken);

            var allExpenses = await _context.ShiftExpenses
                .AsNoTracking()
                .Include(x => x.ShiftTruckEntry)
                .ThenInclude(x => x.Shift)
                .Where(x => !x.IsDeleted &&
                           x.ShiftTruckEntry.Shift.ShiftDate >= DateOnly.FromDateTime(startDate) &&
                           x.ShiftTruckEntry.Shift.ShiftDate <= DateOnly.FromDateTime(endDate))
                .ToListAsync(cancellationToken);

            var allMaintenanceExpenses = await _context.MaintenanceRecords
                .AsNoTracking()
                .Where(x => !x.IsDeleted &&
                           x.MaintenanceDate >= DateOnly.FromDateTime(startDate) &&
                           x.MaintenanceDate <= DateOnly.FromDateTime(endDate))
                .ToListAsync(cancellationToken);

            // جلب جميع أسعار النقلات مرة واحدة للـ caching
            var revenueRates = await _context.RevenueRates
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);

            var chartData = new List<RevenueExpenseChartDto>();

            // معالجة البيانات في الذاكرة
            for (int i = 11; i >= 0; i--)
            {
                var targetDate = currentDate.AddMonths(-i);
                var year = targetDate.Year;
                var month = targetDate.Month;
                var targetDateOnly = DateOnly.FromDateTime(targetDate);

                // حساب الإيرادات من البيانات المحملة
                var monthEntries = allEntries
                    .Where(x => x.Shift.ShiftDate.Year == year && x.Shift.ShiftDate.Month == month)
                    .ToList();

                var revenue = monthEntries.Sum(x =>
                {
                    if (x.TripUnitPrice.HasValue)
                        return x.TripsCount * x.TripUnitPrice.Value;
                    else
                    {
                        var rate = revenueRates
                            .Where(r => r.SiteId == x.Shift.SiteId && r.EffectiveFrom <= x.Shift.ShiftDate)
                            .OrderByDescending(r => r.EffectiveFrom)
                            .FirstOrDefault();
                        return x.TripsCount * (rate?.RatePerTrip ?? 0);
                    }
                });

                // حساب المصروفات من البيانات المحملة
                var expenses = allExpenses
                    .Where(x => x.ShiftTruckEntry.Shift.ShiftDate.Year == year &&
                               x.ShiftTruckEntry.Shift.ShiftDate.Month == month)
                    .Sum(x => x.Amount);

                var maintenanceExpenses = allMaintenanceExpenses
                    .Where(x => x.MaintenanceDate.Year == year && x.MaintenanceDate.Month == month)
                    .Sum(x => x.TotalCost);

                var totalExpenses = expenses + maintenanceExpenses;

                chartData.Add(new RevenueExpenseChartDto
                {
                    Month = $"{months[month - 1]} {year}",
                    Revenue = revenue,
                    Expenses = totalExpenses
                });
            }

            return chartData;
        }

        private async Task<FleetStatusDto> CalculateFleetStatusAsync(CancellationToken cancellationToken)
        {
            // جلب جميع الحالات بشكل متسلسل (DbContext ليس thread-safe)
            var activeTrucks = await _context.DumpTrucks
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Status == DumpTruckStatus.Active)
                .CountAsync(cancellationToken);

            var maintenanceTrucks = await _context.DumpTrucks
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Status == DumpTruckStatus.UnderMaintenance)
                .CountAsync(cancellationToken);

            var inactiveTrucks = await _context.DumpTrucks
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Status == DumpTruckStatus.Inactive)
                .CountAsync(cancellationToken);

            return new FleetStatusDto
            {
                ActiveTrucks = activeTrucks,
                MaintenanceTrucks = maintenanceTrucks,
                InactiveTrucks = inactiveTrucks
            };
        }

        private async Task<List<RecentActivityDto>> GetRecentActivitiesAsync(CancellationToken cancellationToken)
        {
            var activities = new List<RecentActivityDto>();

            // جلب جميع الأنشطة بشكل متسلسل (DbContext ليس thread-safe)
            var recentShifts = await _context.Shifts
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.ShiftDate)
                .ThenByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new RecentActivityDto
                {
                    ActivityType = "ShiftStart",
                    Title = "بدء وردية جديدة",
                    Description = $"وردية بتاريخ {x.ShiftDate:yyyy-MM-dd}",
                    ActivityDate = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var recentMaintenance = await _context.MaintenanceRecords
                .AsNoTracking()
                .Include(x => x.DumpTruck)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaintenanceDate)
                .ThenByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new RecentActivityDto
                {
                    ActivityType = "MaintenanceComplete",
                    Title = "انتهاء الصيانة",
                    Description = $"صيانة للقلاب {x.DumpTruck.TruckNumber} - التكلفة: {x.TotalCost:C}",
                    ActivityDate = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var trucksInMaintenance = await _context.DumpTrucks
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.Status == DumpTruckStatus.UnderMaintenance)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new RecentActivityDto
                {
                    ActivityType = "ProblemAlert",
                    Title = "قلاب في الصيانة",
                    Description = $"قلاب {x.TruckNumber} تحت الصيانة",
                    ActivityDate = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            var recentPayments = await _context.ShiftTruckEntries
                .AsNoTracking()
                .Include(x => x.DumpTruck)
                .Include(x => x.Shift)
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Take(5)
                .Select(x => new RecentActivityDto
                {
                    ActivityType = "PaymentReceipt",
                    Title = "استلام دفعة",
                    Description = $"دفعة للقلاب {x.DumpTruck.TruckNumber} - عدد النقلات: {x.TripsCount}",
                    ActivityDate = x.CreatedAt
                })
                .ToListAsync(cancellationToken);

            activities.AddRange(recentShifts);
            activities.AddRange(recentMaintenance);
            activities.AddRange(trucksInMaintenance);
            activities.AddRange(recentPayments);

            // ترتيب حسب التاريخ (الأحدث أولاً) وأخذ آخر 10
            return activities
                .OrderByDescending(x => x.ActivityDate)
                .Take(10)
                .ToList();
        }

        private decimal GetRevenueRateForShift(Guid siteId, DateOnly shiftDate)
        {
            // البحث عن سعر النقلة الفعال لهذا الموقع في تاريخ الوردية
            var rate = _context.RevenueRates
                .AsNoTracking()
                .Where(x => !x.IsDeleted &&
                           x.SiteId == siteId &&
                           x.EffectiveFrom <= shiftDate)
                .OrderByDescending(x => x.EffectiveFrom)
                .FirstOrDefault();

            return rate?.RatePerTrip ?? 0;
        }

        private async Task<List<TripsChartDto>> CalculateTripsChartAsync(CancellationToken cancellationToken)
        {
            var currentDate = DateTime.Now;
            var months = new[] { "يناير", "فبراير", "مارس", "أبريل", "مايو", "يونيو", "يوليو", "أغسطس", "سبتمبر", "أكتوبر", "نوفمبر", "ديسمبر" };

            // حساب نطاق التواريخ (آخر 12 شهر)
            var startDate = currentDate.AddMonths(-11).Date;
            var endDate = currentDate.Date;

            // جلب جميع البيانات دفعة واحدة بدلاً من loop
            var allTrips = await _context.ShiftTruckEntries
                .AsNoTracking()
                .Include(x => x.Shift)
                .Where(x => !x.IsDeleted &&
                           x.Shift.ShiftDate >= DateOnly.FromDateTime(startDate) &&
                           x.Shift.ShiftDate <= DateOnly.FromDateTime(endDate))
                .Select(x => new { x.Shift.ShiftDate, x.TripsCount })
                .ToListAsync(cancellationToken);

            var chartData = new List<TripsChartDto>();

            // معالجة البيانات في الذاكرة
            for (int i = 11; i >= 0; i--)
            {
                var targetDate = currentDate.AddMonths(-i);
                var year = targetDate.Year;
                var month = targetDate.Month;

                var tripsCount = allTrips
                    .Where(x => x.ShiftDate.Year == year && x.ShiftDate.Month == month)
                    .Sum(x => x.TripsCount);

                chartData.Add(new TripsChartDto
                {
                    Month = $"{months[month - 1]} {year}",
                    TripsCount = tripsCount
                });
            }

            return chartData;
        }

        private async Task<List<TopDriverDto>> GetTopDriversAsync(CancellationToken cancellationToken)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            // حساب الإيرادات بشكل منفصل لأن GetRevenueRateForShift لا يمكن استخدامه في LINQ
            var entries = await _context.ShiftTruckEntries
                .AsNoTracking()
                .Include(x => x.Driver)
                .Include(x => x.Shift)
                .Where(x => !x.IsDeleted &&
                           x.DriverId.HasValue &&
                           x.Driver != null &&
                           x.Shift.ShiftDate.Year == currentYear &&
                           x.Shift.ShiftDate.Month == currentMonth)
                .ToListAsync(cancellationToken);

            // جلب أسعار النقلات مرة واحدة
            var revenueRates = await _context.RevenueRates
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);

            var grouped = entries
                .Where(x => x.DriverId.HasValue && x.Driver != null)
                .GroupBy(x => new { x.DriverId!.Value, DriverName = x.Driver!.FullName })
                .Select(g => new TopDriverDto
                {
                    DriverId = g.Key.Value,
                    DriverName = g.Key.DriverName,
                    TotalTrips = g.Sum(x => x.TripsCount),
                    TotalRevenue = g.Sum(x =>
                    {
                        if (x.TripUnitPrice.HasValue)
                            return x.TripsCount * x.TripUnitPrice.Value;
                        else
                        {
                            var rate = revenueRates
                                .Where(r => r.SiteId == x.Shift.SiteId && r.EffectiveFrom <= x.Shift.ShiftDate)
                                .OrderByDescending(r => r.EffectiveFrom)
                                .FirstOrDefault();
                            return x.TripsCount * (rate?.RatePerTrip ?? 0);
                        }
                    })
                })
                .OrderByDescending(x => x.TotalTrips)
                .Take(5)
                .ToList();

            return grouped;
        }

        private async Task<List<TopTruckDto>> GetTopTrucksAsync(CancellationToken cancellationToken)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var entries = await _context.ShiftTruckEntries
                .AsNoTracking()
                .Include(x => x.DumpTruck)
                .Include(x => x.Shift)
                .Where(x => !x.IsDeleted &&
                           x.Shift.ShiftDate.Year == currentYear &&
                           x.Shift.ShiftDate.Month == currentMonth)
                .ToListAsync(cancellationToken);

            // جلب أسعار النقلات مرة واحدة
            var revenueRates = await _context.RevenueRates
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);

            var grouped = entries
                .GroupBy(x => new { x.DumpTruckId, TruckNumber = x.DumpTruck.TruckNumber })
                .Select(g => new TopTruckDto
                {
                    TruckId = g.Key.DumpTruckId,
                    TruckNumber = g.Key.TruckNumber,
                    TotalTrips = g.Sum(x => x.TripsCount),
                    TotalRevenue = g.Sum(x =>
                    {
                        if (x.TripUnitPrice.HasValue)
                            return x.TripsCount * x.TripUnitPrice.Value;
                        else
                        {
                            var rate = revenueRates
                                .Where(r => r.SiteId == x.Shift.SiteId && r.EffectiveFrom <= x.Shift.ShiftDate)
                                .OrderByDescending(r => r.EffectiveFrom)
                                .FirstOrDefault();
                            return x.TripsCount * (rate?.RatePerTrip ?? 0);
                        }
                    })
                })
                .OrderByDescending(x => x.TotalTrips)
                .Take(5)
                .ToList();

            return grouped;
        }

        private async Task<List<SiteStatisticsDto>> GetSiteStatisticsAsync(CancellationToken cancellationToken)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var entries = await _context.ShiftTruckEntries
                .AsNoTracking()
                .Include(x => x.Shift)
                .ThenInclude(x => x.Site)
                .Where(x => !x.IsDeleted &&
                           x.Shift.ShiftDate.Year == currentYear &&
                           x.Shift.ShiftDate.Month == currentMonth)
                .ToListAsync(cancellationToken);

            var shifts = await _context.Shifts
                .AsNoTracking()
                .Include(x => x.Site)
                .Where(x => !x.IsDeleted &&
                           x.ShiftDate.Year == currentYear &&
                           x.ShiftDate.Month == currentMonth)
                .ToListAsync(cancellationToken);

            // جلب أسعار النقلات مرة واحدة
            var revenueRates = await _context.RevenueRates
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);

            var siteStats = entries
                .GroupBy(x => new { x.Shift.SiteId, SiteName = x.Shift.Site.Name })
                .Select(g => new SiteStatisticsDto
                {
                    SiteId = g.Key.SiteId,
                    SiteName = g.Key.SiteName,
                    MonthlyTrips = g.Sum(x => x.TripsCount),
                    MonthlyRevenue = g.Sum(x =>
                    {
                        if (x.TripUnitPrice.HasValue)
                            return x.TripsCount * x.TripUnitPrice.Value;
                        else
                        {
                            var rate = revenueRates
                                .Where(r => r.SiteId == x.Shift.SiteId && r.EffectiveFrom <= x.Shift.ShiftDate)
                                .OrderByDescending(r => r.EffectiveFrom)
                                .FirstOrDefault();
                            return x.TripsCount * (rate?.RatePerTrip ?? 0);
                        }
                    }),
                    MonthlyShifts = shifts.Count(s => s.SiteId == g.Key.SiteId)
                })
                .OrderByDescending(x => x.MonthlyRevenue)
                .ToList();

            return siteStats;
        }
    }
}
