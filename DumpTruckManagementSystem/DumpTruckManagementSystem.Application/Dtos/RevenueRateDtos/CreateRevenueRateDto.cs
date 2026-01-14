namespace DumpTruckManagementSystem.Application.Dtos.RevenueRateDtos
{
    public class CreateRevenueRateDto
    {
        public Guid SiteId { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public decimal RatePerTrip { get; set; }
        public string CurrencyCode { get; set; } = "SAR";
        public decimal? ExchangeRateToSar { get; set; }
        public string? Notes { get; set; }
    }
}
