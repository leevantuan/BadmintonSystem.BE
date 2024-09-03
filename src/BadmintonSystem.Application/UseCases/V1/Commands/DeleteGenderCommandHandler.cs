using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;
using BadmintonSystem.Domain.Abstractions;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands;
public sealed class DeleteGenderCommandHandler : ICommandHandler<Command.DeleteGenderCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepositoryBase<Gender, Guid> _genderRepository;

    public DeleteGenderCommandHandler(IUnitOfWork unitOfWork,
                                      IRepositoryBase<Gender, Guid> genderRepository)
    {
        _unitOfWork = unitOfWork;
        _genderRepository = genderRepository;
    }

    public async Task<Result> Handle(Command.DeleteGenderCommand request, CancellationToken cancellationToken)
    {
        // Find By Id
        var gender = await _genderRepository.FindByIdAsync(request.Id) ??
            throw new GenderException.GenderNotFoundException(request.Id);

        _genderRepository.Remove(gender);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
