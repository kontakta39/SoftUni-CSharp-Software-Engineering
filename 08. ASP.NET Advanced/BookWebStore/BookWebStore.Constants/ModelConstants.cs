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

    public const string AuthorBirthDateRegex = @"^(1[0-9]{3}|20(0[0-9]|1[0-9]|2[0-4]))-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$";

    public const string AuthorWebsiteRegex = @"^(https?:\/\/)?([\w\-]+\.)+[\w\-]{2,}\/?$";

    public const int BookTitleMinLength = 5;
    public const int BookTitleMaxLength = 50;

    public const int BookPublisherMinLength = 5;
    public const int BookPublisherMaxLength = 50;

    public const int BookMinReleaseYear = 1000;
    public const int BookMaxReleaseYear = 2024;

    public const int BookPagesNumberMinLength = 50;
    public const int BookPagesNumberMaxLength = 1000;

    public const double BookMinPrice = 1.00;
    public const double BookMaxPrice = 200.00;

    public const int BookStockMinLength = 1;
    public const int BookStockMaxLength = 100;

    public const int OrderMinQuantity = 1;
    public const int OrderMaxQuantity = 100;

    public const double OrderMinPrice = 1.00;
    public const double OrderMaxPrice = 200.00;

    public const int ReviewRatingMinLength = 1;
    public const int ReviewRatingMaxLength = 5;

    public const int ReviewTextMinLength = 10;
    public const int ReviewTextMaxLength = 500;

    public const int BlogTitleMinLength = 1;
    public const int BlogTitleMaxLength = 50;

    public const int BlogContentMinLength = 200;
}