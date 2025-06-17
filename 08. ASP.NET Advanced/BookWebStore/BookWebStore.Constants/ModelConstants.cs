namespace BookWebStore.Constants;
public static class ModelConstants
{
    public const int ApplicationUserNameMinLength = 3;
    public const int ApplicationUserNameMaxLength = 20;

    public const int PasswordMinLength = 6;
    public const int PasswordMaxLength = 20;

    public const string PhoneNumberRegex = @"^\+359\d{9}$";
}