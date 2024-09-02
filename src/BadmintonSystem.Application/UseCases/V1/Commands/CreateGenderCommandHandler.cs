using BadmintonSystem.Contract.Abstractions.Messages;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.Gender;

namespace BadmintonSystem.Application.UseCases.V1.Commands;
public class CreateGenderCommandHandler : ICommandHandler<Command.CreateGenderCommand>
{
    public Task<Result> Handle(Command.CreateGenderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
