using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Data.Models;

public class ProductClient
{
    [Key]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; }

    [Key]
    public int ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }
}

//•	ProductId – integer, Primary Key, foreign key(required)
//•	Product – Product
//•	ClientId – integer, Primary Key, foreign key(required)
//•	Client – Client
