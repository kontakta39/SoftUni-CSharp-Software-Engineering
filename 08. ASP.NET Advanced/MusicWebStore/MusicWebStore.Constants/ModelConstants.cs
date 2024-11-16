﻿namespace MusicWebStore.Constants;

public static class ModelConstants
{
    public const int ArtistNameMinLength = 2;
    public const int ArtistNameMaxLength = 60;

    public const int ArtistBiographyMinLength = 10;
    public const int ArtistBiographyMaxLength = 500;

    public const int ArtistNationalityMinLength = 5;
    public const int ArtistNationalityMaxLength = 30;

    public const int ArtistWebsiteMinLength = 5;
    public const int ArtistWebsiteMaxLength = 300;

    public const int GenreNameMinLength = 2;
    public const int GenreNameMaxLength = 30;

    public const int AlbumTitleMinLength = 1;
    public const int AlbumTitleMaxLength = 100;

    public const int AlbumLabelMinLength = 5;
    public const int AlbumLabelMaxLength = 50;

    public const int AlbumDescriptionMinLength = 10;
    public const int AlbumDescriptionMaxLength = 300;

    public const double AlbumMinPrice = 1.00;
    public const double AlbumMaxPrice = 3000.00;

    public const int AlbumStockMinLength = 1;
    public const int AlbumStockMaxLength = 50;

    public const int ReviewTextMinLength = 10;
    public const int ReviewTextMaxLength = 500;

    public const int RatingMinLength = 1;
    public const int RatingMaxLength = 10;

    public const int OrderQuantityMinLength = 1;
    public const int OrderQuantityMaxLength = 50;
}