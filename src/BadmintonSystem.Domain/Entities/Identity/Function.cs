using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities.Identity;
public class Function : EntityAuditBase<int>
{
    public string Name { get; set; }

    public string Url { get; set; }

    public int? ParentId { get; set; }

    public int? SortOrder { get; set; }

    public string? CssClass { get; set; } // icon in menu

    public FunctionStatus Status { get; set; }

    public string Key { get; set; }

    public int ActionValue { get; set; }
}
