using BadmintonSystem.Contract.Constants.Models;

namespace BadmintonSystem.Contract.Services.V2.Club;
public static class Request
{
    public class ClubRequest : BaseEntityDto<Guid>
    {
        public string? Code { get; set; }
        public string? HotLine { get; set; }
        public string? FacebookPageLink { get; set; }
        public string? InstagramPageLink { get; set; }
        public string? MapLink { get; set; }
        public string? ImageLink { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }
    }

    public class ClubPaginationRequest : PaginationRequest
    {
    }
}
