using BadmintonSystem.Contract.Constants.Models;

namespace BadmintonSystem.Contract.Services.V2.AdditionalService;
public static class Request
{
    public class AdditionalServiceRequest : BaseEntityDto<Guid>
    {
        public decimal Price { get; set; }
        public Guid? ClubsId { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class AdditionalServicePaginationRequest : PaginationRequest
    {
    }
}
