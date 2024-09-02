using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Gender : AuditableEntity<Guid>
{
    public string Name { get; set; }
}
