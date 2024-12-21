using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class Price : EntityAuditBase<Guid>
{
    public decimal YardPrice { get; set; }

    public DefaultEnum IsDefault { get; set; }

    public virtual ICollection<YardType>? YardTypes { get; set; }

    public virtual ICollection<YardPrice>? YardPrices { get; set; }
}
