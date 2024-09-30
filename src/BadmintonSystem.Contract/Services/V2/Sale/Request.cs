using BadmintonSystem.Contract.Constants.Models;

namespace BadmintonSystem.Contract.Services.V2.Sale;
public static class Request
{
    public class SaleRequest : BaseEntityDto<Guid>
    {
        public decimal? Persent { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? Status { get; set; }
    }
}
