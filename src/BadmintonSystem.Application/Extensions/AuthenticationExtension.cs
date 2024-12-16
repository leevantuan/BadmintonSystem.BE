using System.Security.Claims;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Application.Extensions;

public static class AuthenticationExtension
{
    private static Response.UserAuthorization ActionHandler(string? function, string? value)
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

    public static string ConvertToBinary(string value)
    {
        var listAction = value.Trim().Split(',').ToList();

        int newValue = 0;
        foreach (string action in listAction)
        {
            newValue += 1 << Convert.ToInt32(action);
        }

        return newValue.ToString();
    }

    public static Dictionary<string, string> MergeClaims(Dictionary<string, string> newClaims, IList<Claim> roleClaims)
    {
        foreach (Claim item in roleClaims.ToList())
        {
            if (newClaims.TryGetValue(item.Type, out string value))
            {
                int.TryParse(item.Value, out int newValue);
                int.TryParse(value, out int oldValue);
                newValue |= oldValue; // Or
                newClaims[item.Type] = newValue.ToString();
            }
            else
            {
                newClaims.TryAdd(item.Type, item.Value);
            }
        }

        return newClaims;
    }

    public static Dictionary<string, string> SplitClaims(Dictionary<string, string> newClaims, IList<Claim> roleClaims)
    {
        foreach (Claim item in roleClaims.ToList())
        {
            if (newClaims.TryGetValue(item.Type, out string value))
            {
                int.TryParse(item.Value, out int removeValue);
                int.TryParse(value, out int oldValue);
                int newValue = oldValue &= ~removeValue; // And
                newClaims[item.Type] = newValue.ToString();
            }
        }

        return newClaims;
    }

    public static List<Response.UserAuthorization> GetActionValues(IList<Claim> claims)
    {
        var functionKeys = Enum.GetValues<FunctionEnum>()
            .Select(e => e.ToString())
            .ToList();

        var actionValues = new List<Response.UserAuthorization>();

        foreach (string function in functionKeys)
        {
            try
            {
                string? value = claims.FirstOrDefault(x => x.Type == function).Value;

                if (value != null)
                {
                    Response.UserAuthorization? actionValue = ActionHandler(function, value);

                    if (actionValue != null)
                    {
                        actionValues.Add(actionValue);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        return actionValues;
    }
}
