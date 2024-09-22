namespace BadmintonSystem.Domain.Exceptions;

public class ClubException
{
    public class ClubNotFoundException : NotFoundException
    {
        public ClubNotFoundException(Guid ClubId)
            : base($"===========> The Club with the id {ClubId} was not found.") { }
    }

    public class ClubBadRequestException : BadRequestException
    {
        public ClubBadRequestException(string message)
            : base($"===========> Bad Request: {message}.")
        {
        }
    }
}
