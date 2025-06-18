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

    public const int AuthorNationalityMinLength = 5;
    public const int AuthorNationalityMaxLength = 30;

    public const int AuthorWebsiteMinLength = 5;
    public const int AuthorWebsiteMaxLength = 300;
}