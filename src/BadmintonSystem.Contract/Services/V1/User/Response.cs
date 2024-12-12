using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Response
{
    public class AppUserResponse : EntityBase<Guid>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string AvatarUrl { get; set; }
    }
}
