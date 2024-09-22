using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V2.AdditionalService;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V2.AdditionalService.Commands;
public sealed class UpdateAdditionalServiceCommandHandler : ICommandHandler<Command.UpdateAdditionalServiceCommand>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Domain.Entities.AdditionalService, Guid> _additionalServiceRepository;

    public UpdateAdditionalServiceCommandHandler(IMapper mapper,
                                      IUnitOfWork unitOfWork,
                                      IRepositoryBase<Domain.Entities.AdditionalService, Guid> additionalServiceRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _additionalServiceRepository = additionalServiceRepository;
    }

    public async Task<Result> Handle(Command.UpdateAdditionalServiceCommand request, CancellationToken cancellationToken)
    {
        var additionalService = await _additionalServiceRepository.FindByIdAsync(request.Id) ??
            throw new AdditionalServiceException.AdditionalServiceNotFoundException(request.Id);

        var isNameExists = await _additionalServiceRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();

        if (isNameExists.Any())
            return Result.Failure(new Error("200", "Is Name Exists!"));

        additionalService.UpdateAdditionalService(request.Data.Name, request.Data.Price, request.Data.ClubsId, request.Data.CategoryId);

        //await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
