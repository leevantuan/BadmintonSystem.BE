using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Gender;
using BadmintonSystem.Domain.Abstractions.Dappers;

namespace BadmintonSystem.Application.UseCases.V2.Queries;
public sealed class GetAllGenderQueryHandler : IQueryHandler<Query.GetAllGender, List<Response.GenderResponse>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetAllGenderQueryHandler(IMapper mapper,
                                    IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<Response.GenderResponse>>> Handle(Query.GetAllGender request, CancellationToken cancellationToken)
    {
        var gender = await _unitOfWork.Genders.GetAllAsync();

        var result = _mapper.Map<List<Response.GenderResponse>>(gender);

        return result;
    }
}
