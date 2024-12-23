using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Queries.Price;

public sealed class GetPriceByIdQueryHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.Price, Guid> priceRepository)
    : IQueryHandler<Query.GetPriceByIdQuery, Response.PriceDetailResponse>
{
    public async Task<Result<Response.PriceDetailResponse>> Handle
        (Query.GetPriceByIdQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.Price price = await priceRepository.FindByIdAsync(request.Id, cancellationToken)
                                      ?? throw new PriceException.PriceNotFoundException(request.Id);

        Response.PriceDetailResponse? result = mapper.Map<Response.PriceDetailResponse>(price);

        result.IsDefault = (int)price.IsDefault;

        return Result.Success(result);
    }
}
