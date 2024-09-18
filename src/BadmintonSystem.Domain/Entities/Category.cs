using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Category : AuditableEntity<Guid>
{
    public string Name { get; private set; }
    public virtual ICollection<AdditionalService> AdditionalServices { get; private set; }

    public static Category CreateCategory(string name)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            AdditionalServices = new List<AdditionalService>()
        };
    }

    public void UpdateCategory(string name)
    {
        Name = name;
    }
}
