using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities.Identity;
public class Action : EntityAuditBase<int>
{
    public string Name { get; set; }

    public int? SortOrder { get; set; }

    public bool? IsActive { get; set; }
}
