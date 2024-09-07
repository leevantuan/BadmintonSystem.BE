using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.Exceptions;

// Override custom nhìn cho dễ
// Được kế thừa từ DomainException request 2 field : Title, Message
public sealed class ValidationException : DomainException
{
    // Step 5: Nếu kh truyền gì thì sẽ output defaul "Validation Failure", "One or more validation errors occurred"
    // Roll back ============> Handler in Appilcation.UseCase
    public ValidationException(IReadOnlyCollection<ValidationError> errors)
        : base("Validation Failure", "One or more validation errors occurred")
        => Errors = errors;

    public IReadOnlyCollection<ValidationError> Errors { get; }
}

public record ValidationError(string PropertyName, string ErrorMessage);
