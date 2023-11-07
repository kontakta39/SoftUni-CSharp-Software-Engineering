using ValidationAttributes.Attributes;

namespace ValidationAttributes.Utils;

public static class Validator
{
    public static bool IsValid(object obj)
    {
        var properties = obj.GetType().GetProperties();

        foreach (var property in properties)
        {
            var attributes = property.GetCustomAttributes(typeof(MyValidationAttribute), true);

            foreach (var attribute in attributes)
            {
                if (attribute is MyValidationAttribute validationAttribute)
                {
                    var value = property.GetValue(obj);
                    if (!validationAttribute.IsValid(value))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
}