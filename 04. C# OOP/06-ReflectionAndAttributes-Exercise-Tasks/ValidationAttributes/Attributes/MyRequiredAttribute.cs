namespace ValidationAttributes.Attributes;

public class MyRequiredAttribute : MyValidationAttribute
{
    public override bool IsValid(object obj)
    //{
    //    if ((string)obj == null)
    //    {
    //        return false;
    //    }

    //    return true;
    //}
    => obj != null;
}