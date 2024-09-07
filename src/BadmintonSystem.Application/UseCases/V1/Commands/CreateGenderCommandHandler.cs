using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands;
public sealed class CreateGenderCommandHandler : ICommandHandler<Command.CreateGenderCommand>
{
    // Step 3: Call RepositoryBase in Persistence to get Database
    // Before handler logic
    // ==> PipelineBehavior == [Middleware wrap]
    // Performance == đo request or response mất bao lâu
    // Tracing ==> log có thành công hay không - mất bao nhiêu lâu
    // Validation Default && Validation ==>  Check Rule
    // If validator have error ==> Middleware ==> GetErrors()
    // Application.Exceptions.ValidationException
    // If not error ==> Handler
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRepositoryBase<Gender, Guid> _genderRepository;

    public CreateGenderCommandHandler(IUnitOfWork unitOfWork,
                                      IMapper mapper,
                                      IRepositoryBase<Gender, Guid> genderRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _genderRepository = genderRepository;
    }

    public async Task<Result> Handle(Command.CreateGenderCommand request, CancellationToken cancellationToken)
    {
        // Is Name Exists
        var isNameExists = await _genderRepository.FindAll(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim())).ToListAsync();
        //var isNameExists = await _genderRepository.FindSingleAsync(x => x.Name.ToLower().Trim().Equals(request.Data.Name.ToLower().Trim()));
        if (isNameExists != null)
            throw new GenderException.GenderBadRequestException("Name Exists!");

        // Map data into Entities
        var gender = _mapper.Map<Gender>(request.Data);

        _genderRepository.Add(gender);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
