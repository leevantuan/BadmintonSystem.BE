namespace BadmintonSystem.Domain.Entities.Identity;
public class PermissionInUser
{
    public Guid UserId { get; set; }
    public string FunctionId { get; set; }
    public int BinaryValue { get; set; }
}
