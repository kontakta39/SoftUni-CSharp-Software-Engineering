﻿namespace DeskMarket.Models;

public class CartViewModel
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
}