﻿namespace CarDealer.DTOs.Import;

public class SaleDTO
{
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public decimal Discount { get; set; }
}