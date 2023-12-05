using System.ComponentModel.DataAnnotations;

namespace Invoices.Data.Models;

public class Client
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(25, MinimumLength = 10)]
    public string Name { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 10)]
    public string NumberVat { get; set; }

    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public ICollection<Address> Addresses { get; set; } = new List<Address>();
    public ICollection<ProductClient> ProductsClients { get; set; } = new List<ProductClient>();
}

//•	Id – integer, Primary Key
//•	Name – text with length[10…25] (required)
//•	NumberVat – text with length[10…15] (required)
//•	Invoices – collection of type Invoicе
//•	Addresses – collection of type Address
//•	ProductsClients – collection of type ProductClient