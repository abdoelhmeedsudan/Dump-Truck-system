using AutoMapper;
using DumpTruckManagementSystem.Application.Dtos.DriverDtos;
using DumpTruckManagementSystem.Application.Dtos.DumpTruckDtos;
using DumpTruckManagementSystem.Application.Dtos.ExpenseTypeDtos;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceRecordDtos;
using DumpTruckManagementSystem.Application.Dtos.MaintenanceTypeDtos;
using DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos;
using DumpTruckManagementSystem.Application.Dtos.ShiftDtos;
using DumpTruckManagementSystem.Application.Dtos.ShiftExpenseDtos;
using DumpTruckManagementSystem.Application.Dtos.ShiftTruckEntryDtos;
using DumpTruckManagementSystem.Application.Dtos.SiteDtos;
using DumpTruckManagementSystem.Domain.Entities;

namespace DumpTruckManagementSystem.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // DumpTruck Mappings
            CreateMap<CreateDumpTruckDto, DumpTruck>();
            CreateMap<UpdateDumpTruckDto, DumpTruck>();
            CreateMap<DumpTruck, DumpTruckDto>();

            // Driver Mappings
            CreateMap<CreateDriverDto, Driver>();
            CreateMap<UpdateDriverDto, Driver>();
            CreateMap<Driver, DriverDto>();

            // Site Mappings
            CreateMap<CreateSiteDto, Site>();
            CreateMap<UpdateSiteDto, Site>();
            CreateMap<Site, SiteDto>();

            // Shift Mappings
            CreateMap<CreateShiftDto, Shift>();
            CreateMap<UpdateShiftDto, Shift>();
            CreateMap<Shift, ShiftDto>();

            // ShiftTruckEntry Mappings
            CreateMap<CreateShiftTruckEntryDto, ShiftTruckEntry>();
            CreateMap<UpdateShiftTruckEntryDto, ShiftTruckEntry>();
            CreateMap<ShiftTruckEntry, ShiftTruckEntryDto>();

            // ShiftExpense Mappings
            CreateMap<CreateShiftExpenseDto, ShiftExpense>();
            CreateMap<UpdateShiftExpenseDto, ShiftExpense>();
            CreateMap<ShiftExpense, ShiftExpenseDto>();

            // ExpenseType Mappings
            CreateMap<CreateExpenseTypeDto, ExpenseType>();
            CreateMap<UpdateExpenseTypeDto, ExpenseType>();
            CreateMap<ExpenseType, ExpenseTypeDto>();

            // MaintenanceType Mappings
            CreateMap<CreateMaintenanceTypeDto, MaintenanceType>();
            CreateMap<UpdateMaintenanceTypeDto, MaintenanceType>();
            CreateMap<MaintenanceType, MaintenanceTypeDto>();

            // MaintenanceRecord Mappings
            CreateMap<CreateMaintenanceRecordDto, MaintenanceRecord>();
            CreateMap<UpdateMaintenanceRecordDto, MaintenanceRecord>();
            CreateMap<MaintenanceRecord, MaintenanceRecordDto>();

            // RevenueRate Mappings
            CreateMap<CreateRevenueRateDto, RevenueRate>();
            CreateMap<UpdateRevenueRateDto, RevenueRate>();
            CreateMap<RevenueRate, RevenueRateDto>();
        }
    }
}
