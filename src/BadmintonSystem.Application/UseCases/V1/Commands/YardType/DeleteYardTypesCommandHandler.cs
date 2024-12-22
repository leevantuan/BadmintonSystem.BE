using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardType;

public sealed class DeleteYardTypesCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : ICommandHandler<Command.DeleteYardTypesCommand>
{
    public async Task<Result> Handle(Command.DeleteYardTypesCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.YardType> yardTypes = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.YardType yardType = await yardTypeRepository.FindByIdAsync(idValue, cancellationToken)
                                                ?? throw new YardTypeException.YardTypeNotFoundException(idValue);

            yardTypes.Add(yardType);
        }

        yardTypeRepository.RemoveMultiple(yardTypes);

        return Result.Success();
    }
}
