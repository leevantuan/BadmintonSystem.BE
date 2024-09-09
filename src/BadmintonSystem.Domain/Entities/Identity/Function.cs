namespace BadmintonSystem.Domain.Entities.Identity;
public class Function
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string? ParrentId { get; set; }
    public int? SortOrder { get; set; }
    public string CssClass { get; set; }
    public bool? IsActive { get; set; }

    public virtual ICollection<ActionInFunction> ActionInFunctions { get; set; }
    public virtual ICollection<PermissionInRole> PermissionInRoles { get; set; }
    public virtual ICollection<PermissionInUser> PermissionInUsers { get; set; }
}
