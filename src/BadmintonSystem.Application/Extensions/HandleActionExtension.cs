using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Application.Extensions;

public static class HandleActionExtension
{
    public static Response.UserAuthorization ActionHandler(string? function, string? value)
    {
        if (string.IsNullOrEmpty(function))
        {
            return null;
        }

        var result = new Response.UserAuthorization
        {
            FunctionKey = function,
            Action = new List<string>()
        };

        if (string.IsNullOrEmpty(value) || !int.TryParse(value, out int actionUserValue))
        {
            return result;
        }

        var actionsEnum = Enum.GetValues<ActionEnum>()
            .Select(e => (int)e)
            .ToList();

        foreach (int action in actionsEnum)
        {
            int actionValueInitial = action;
            int actionValue = 1 << actionValueInitial;

            if ((actionUserValue & actionValue) == actionValue)
            {
                result.Action.Add(actionValueInitial.ToString());
            }
        }

        return result;
    }
}
