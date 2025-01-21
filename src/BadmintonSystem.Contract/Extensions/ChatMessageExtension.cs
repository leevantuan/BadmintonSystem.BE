namespace BadmintonSystem.Contract.Extensions;

public static class ChatMessageExtension
{
    public static string GetSortChatMessageProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "content" => "Content",
            _ => "CreatedDate"
        };
    }
}
