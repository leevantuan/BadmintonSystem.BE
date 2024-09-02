namespace BadmintonSystem.Contract.Constants.Models;
public class AddOrUpdateRequest<T>
{
    public virtual T? Id { get; set; }
    public Dictionary<string, string>? Data { get; set; }
    public Guid? User_ApplicationUserIdCreated { get; set; }
    public Guid? User_ApplicationUserIdModified { get; set; }
}
