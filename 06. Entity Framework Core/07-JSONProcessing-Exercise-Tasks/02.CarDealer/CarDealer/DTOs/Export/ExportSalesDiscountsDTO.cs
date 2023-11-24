using CarDealer.Models;

namespace CarDealer.DTOs.Export;

public class ExportSalesDiscountsDTO
{
    public decimal Discount { get; set; }
    public Car Car { get; set; }
    public Customer Customer { get; set; }
}