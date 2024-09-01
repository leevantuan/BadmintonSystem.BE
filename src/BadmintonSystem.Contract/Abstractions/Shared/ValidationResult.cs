namespace BadmintonSystem.Contract.Abstractions.Shared;
public sealed class ValidationResult : Result, IValidationResult
{
    // If have error then will truyền vào mã lỗi => Error
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError) =>
        Errors = errors;

    public Error[] Errors { get; }

    // Return list Errors
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}
