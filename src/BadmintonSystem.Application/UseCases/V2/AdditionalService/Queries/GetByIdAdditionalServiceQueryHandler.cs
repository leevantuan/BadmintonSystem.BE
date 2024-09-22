using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.AdditionalService;
using BadmintonSystem.Domain.Abstractions.Dappers;

namespace BadmintonSystem.Application.UseCases.V2.AdditionalService.Queries;
public sealed class GetByIdAdditionalServiceQueryHandler : IQueryHandler<Query.GetAdditionalServiceByIdQuery, Response.AdditionalServiceResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdAdditionalServiceQueryHandler(IMapper mapper,
                                    IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Response.AdditionalServiceResponse>> Handle(Query.GetAdditionalServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var additionalService = await _unitOfWork.AdditionalServices.GetByIdAsync(request.Id);

        var result = _mapper.Map<Response.AdditionalServiceResponse>(additionalService);

        return result;
    }
}
