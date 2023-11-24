using CarDealer.Models;

namespace CarDealer.DTOs.Export;

public class ExportCustomerTotalSales
{
    public string Name { get; set; }
    public ICollection<Sale> Sales { get; set; }
}