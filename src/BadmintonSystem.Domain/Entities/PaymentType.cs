using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class PaymentType : EntityAuditBase<Guid>
{
    public string Name { get; set; }

    public virtual ICollection<PaymentMethod>? PaymentMethods { get; set; }
}
