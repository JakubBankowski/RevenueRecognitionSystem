namespace RevenueRecognitionSystem.Services.Revenue;

public interface IRevenueService
{
    Task<decimal> GetCurrentRevenueAsync(int? productId);
    Task<decimal> GetPredictedRevenueAsync(int? productId);
}