using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Response
{
    // USER DETAIL
    public class AppUserResponse : EntityBase<Guid>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public int Gender { get; set; }

        public string AvatarUrl { get; set; }
    }

    // Address by user
    public class AddressByUserDetailResponse : Address.Response.AddressResponse
    {
        public int IsDefault { get; set; }
    }

    public class AddressByUserSql
    {
    }
}
