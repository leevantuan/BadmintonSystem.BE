namespace BadmintonSystem.Application.UseCases.V1.Services;

public interface IYardPriceService
{
    Task<bool> CreateYardPrice(DateTime date, Guid userId);
}
