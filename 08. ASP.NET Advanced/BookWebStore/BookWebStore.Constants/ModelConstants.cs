namespace BookWebStore.Constants;
public static class ModelConstants
{
    public const int ApplicationUserNameMinLength = 3;
    public const int ApplicationUserNameMaxLength = 20;

    public const int PasswordMinLength = 6;
    public const int PasswordMaxLength = 20;

    public const string PhoneNumberRegex = @"^\+359\d{9}$";

    public const int GenreNameMinLength = 2;
    public const int GenreNameMaxLength = 30;

    public const int AuthorNameMinLength = 2;
    public const int AuthorNameMaxLength = 60;

    public const int AuthorBiographyMinLength = 10;
    public const int AuthorBiographyMaxLength = 1000;

    public const string AuthorBirthDateRegex = @"^(1[0-9]{3}|20(0[0-9]|1[0-9]|2[0-5]))-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$";

    public const string AuthorWebsiteRegex = @"^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}\/?$";
}