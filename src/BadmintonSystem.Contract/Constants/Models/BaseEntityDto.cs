namespace BadmintonSystem.Contract.Constants.Models;

public class BaseEntityDto<T>
{
    public virtual T? Id { get; set; }
    public string? Name { get; set; }
    public string? DateCreated { get; set; }
    public string? DateLastModified { get; set; }
    public bool? DeleteFlag { get; set; }
    public string? UserCreated { get; set; }
    public string? UserLastModified { get; set; }
    public Guid? User_ApplicationUserIdCreated { get; set; }
    public Guid? User_ApplicationUserIdModified { get; set; }
    public bool? Default { get; set; }
}
