using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;

namespace BadmintonSystem.Application.UseCases.V1.Commands;
public class UpdateGenderCommandHandler : ICommandHandler<Command.UpdateGenderCommand>
{
    public Task<Result> Handle(Command.UpdateGenderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
