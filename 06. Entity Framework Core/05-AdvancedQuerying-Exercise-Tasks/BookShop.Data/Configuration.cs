﻿namespace BookShop.Data;

internal class Configuration
{
    internal static string ConnectionString
        => @"Server=ACER\SQLEXPRESS;Database=BookShop;Integrated Security=True;";
}