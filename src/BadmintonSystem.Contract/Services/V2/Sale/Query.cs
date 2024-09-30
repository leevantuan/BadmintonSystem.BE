using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Sale;
public static class Query
{
    public record GetSaleByIdQuery(Guid Id) : IQuery<Response.SaleResponse>;
    public record GetAllSale() : IQuery<List<Response.SaleResponse>>;
}
