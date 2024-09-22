namespace BadmintonSystem.Domain.Exceptions;

public class AdditionalServiceException
{
    public class AdditionalServiceNotFoundException : NotFoundException
    {
        public AdditionalServiceNotFoundException(Guid AdditionalServiceId)
            : base($"===========> The AdditionalService with the id {AdditionalServiceId} was not found.") { }
    }

    public class AdditionalServiceBadRequestException : BadRequestException
    {
        public AdditionalServiceBadRequestException(string message)
            : base($"===========> Bad Request: {message}.")
        {
        }
    }
}
