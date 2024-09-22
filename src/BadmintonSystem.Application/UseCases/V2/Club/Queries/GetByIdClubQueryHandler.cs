using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.Club;
using BadmintonSystem.Domain.Abstractions.Dappers;

namespace BadmintonSystem.Application.UseCases.V2.Club.Queries;
public sealed class GetByIdClubQueryHandler : IQueryHandler<Query.GetClubByIdQuery, Response.ClubResponse>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdClubQueryHandler(IMapper mapper,
                                    IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Response.ClubResponse>> Handle(Query.GetClubByIdQuery request, CancellationToken cancellationToken)
    {
        var club = await _unitOfWork.Clubs.GetByIdAsync(request.Id);

        var result = _mapper.Map<Response.ClubResponse>(club);

        return result;
    }
}
