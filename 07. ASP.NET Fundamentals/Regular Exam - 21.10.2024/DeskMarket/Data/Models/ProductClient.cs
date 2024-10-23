using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DeskMarket.Data.Models;

[PrimaryKey(nameof(ProductId), nameof(ClientId))]
public class ProductClient
{
    [Key]
    [Required]
    public int ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;

    [Key]
    [Required]
    public string ClientId { get; set; } = null!;

    [ForeignKey(nameof(ClientId))]
    public IdentityUser Client { get; set; } = null!;
}