namespace BadmintonSystem.Contract.Abstractions.Shared;
public interface IInternalService
{
    object ValueAttachmentForObject(object newObj, Dictionary<string, string> data, object updateObj);
}
