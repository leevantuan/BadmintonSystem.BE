using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Gender;
public static class Query
{
    public record GetGenderByIdQuery(Guid Id) : IQuery<Response.GenderResponse>;
    public record GetAllGender() : IQuery<List<Response.GenderResponse>>;
}
