using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Yard;

public sealed class DeleteYardsCommandHandler(
    IRepositoryBase<Domain.Entities.Yard, Guid> yardRepository)
    : ICommandHandler<Command.DeleteYardsCommand>
{
    public async Task<Result> Handle(Command.DeleteYardsCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.Yard> yards = new();

        foreach (string id in request.Ids)
        {
            var idValue = Guid.Parse(id);

            Domain.Entities.Yard yard = await yardRepository.FindByIdAsync(idValue, cancellationToken)
                                        ?? throw new YardException.YardNotFoundException(idValue);

            yards.Add(yard);
        }

        yardRepository.RemoveMultiple(yards);

        return Result.Success();
    }
}
