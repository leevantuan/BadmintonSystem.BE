using BadmintonSystem.Domain.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class AdditionalService : AuditableEntity<Guid>
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public Guid? ClubId { get; private set; }
    public Guid? CategoryId { get; private set; }

    public static AdditionalService CreateAdditionalService(string name,
                                                            decimal price,
                                                            Guid? clubId,
                                                            Guid? categoryId)
        => new AdditionalService { Id = Guid.NewGuid(), Name = name, Price = price, ClubId = clubId, CategoryId = categoryId };

    public void UpdateAdditionalService(string name, decimal price, Guid? clubId, Guid? categoryId)
    {
        Name = name;
        Price = price;
        ClubId = clubId;
        CategoryId = categoryId;
    }
}
