using System.Globalization;
using BadmintonSystem.Contract.Constants.API;
using Microsoft.Extensions.Options;

namespace BadmintonSystem.Contract.Abstractions.Shared;
public class InternalService : IInternalService
{
    private readonly DataTypeFormatConstants _dataTypeFormatConstant;

    public InternalService(IOptions<DataTypeFormatConstants> dataTypeFormatConstant)
    {
        _dataTypeFormatConstant = dataTypeFormatConstant.Value;
    }
    public object ValueAttachmentForObject(object newObj,
                                           Dictionary<string, string> data,
                                           object updateObj)
    {
        int intValue;
        double doubleValue;
        bool boolValue;
        DateTime ngayValue;
        TimeSpan gioValue;
        Guid guidValue;

        if (updateObj != null)
        {
            foreach (KeyValuePair<string, string> updateItem in data)
            {
                if (newObj.GetType().GetProperty(updateItem.Key) != null)
                {
                    if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(string))
                    {
                        updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, updateItem.Value, null);
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(Guid))
                    {
                        if (Guid.TryParse(updateItem.Value, out guidValue))
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, guidValue, null);
                        }
                        else
                        {
                            throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not Guid");
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(Guid?))
                    {
                        if (!string.IsNullOrEmpty(updateItem.Value))
                        {
                            if (Guid.TryParse(updateItem.Value, out guidValue))
                            {
                                updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, guidValue, null);
                            }
                            else
                            {
                                throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not Guid");
                            }
                        }
                        else
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, null, null);
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(int))
                    {
                        if (int.TryParse(updateItem.Value, out intValue))
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, intValue, null);
                        }
                        else
                        {
                            throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not Int");
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(int?))
                    {
                        if (!string.IsNullOrEmpty(updateItem.Value))
                        {
                            if (int.TryParse(updateItem.Value, out intValue))
                            {
                                updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, intValue, null);
                            }
                            else
                            {
                                throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not Int");
                            }
                        }
                        else
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, null, null);
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(double))
                    {
                        if (double.TryParse(updateItem.Value, out doubleValue))
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, doubleValue, null);
                        }
                        else
                        {
                            throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not double");
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(double?))
                    {
                        if (!string.IsNullOrEmpty(updateItem.Value))
                        {
                            if (double.TryParse(updateItem.Value, out doubleValue))
                            {
                                updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, doubleValue, null);
                            }
                            else
                            {
                                throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not double");
                            }
                        }
                        else
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, null, null);
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(bool))
                    {
                        if (bool.TryParse(updateItem.Value, out boolValue))
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, boolValue, null);
                        }
                        else
                        {
                            throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not bool");
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(bool?))
                    {
                        if (!string.IsNullOrEmpty(updateItem.Value))
                        {
                            if (bool.TryParse(updateItem.Value, out boolValue))
                            {
                                updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, boolValue, null);
                            }
                            else
                            {
                                throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not bool");
                            }
                        }
                        else
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, null, null);
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParseExact(updateItem.Value, _dataTypeFormatConstant.DatePatternFromString, null, DateTimeStyles.None, out ngayValue))
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, ngayValue, null);
                        }
                        else
                        {
                            throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not correct format: {_dataTypeFormatConstant.DatePatternFromString}");
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(DateTime?))
                    {
                        if (!string.IsNullOrEmpty(updateItem.Value))
                        {
                            if (DateTime.TryParseExact(updateItem.Value, _dataTypeFormatConstant.DatePatternFromString, null, DateTimeStyles.None, out ngayValue))
                            {
                                updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, ngayValue, null);
                            }
                            else
                            {
                                throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not correct format: {_dataTypeFormatConstant.DatePatternFromString}");
                            }
                        }
                        else
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, null, null);
                        }
                    }
                    //Cofiguration Start TimeSpan
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(TimeSpan))
                    {
                        if (TimeSpan.TryParse(updateItem.Value, out gioValue))
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, gioValue, null);
                        }
                        else
                        {
                            throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not correct format TimeSpan.");
                        }
                    }
                    else if (newObj.GetType().GetProperty(updateItem.Key)?.PropertyType == typeof(TimeSpan?))
                    {
                        if (!string.IsNullOrEmpty(updateItem.Value))
                        {
                            if (TimeSpan.TryParse(updateItem.Value, out gioValue))
                            {
                                updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, gioValue, null);
                            }
                            else
                            {
                                throw new ApplicationException($"The value of {updateItem.Key}: {updateItem.Value} is not correct format TimeSpan.");
                            }
                        }
                        else
                        {
                            updateObj.GetType().GetProperty(updateItem.Key)?.SetValue(updateObj, null, null);
                        }
                    }

                    //End Convert TimeSpan
                }
                else
                {
                    throw new ApplicationException($"Field {updateItem.Key} not found");
                }
            }
        }

        return updateObj;
    }
}
