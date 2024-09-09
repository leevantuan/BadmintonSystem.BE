namespace BadmintonSystem.Domain.Entities.Identity;
public class PermissionInRole
{
    public Guid RoleId { get; set; }
    public string FunctionId { get; set; }
    public int BinaryValue { get; set; }
}
