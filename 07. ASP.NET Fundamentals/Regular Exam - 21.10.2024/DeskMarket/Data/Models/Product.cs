using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DeskMarket.Constants.ModelConstants;

namespace DeskMarket.Data.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength)]
    public string ProductName { get; set; } = null!;

    [Required]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)] 
    public string Description { get; set; } = null!;

    [Required]
    [Range(MinPrice, MaxPrice)]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public string SellerId { get; set; } = null!;

    [Required]
    public IdentityUser Seller { get; set; } = null!;

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public DateTime AddedOn { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = null!;

    public bool IsDeleted { get; set; } = false;

    public ICollection<ProductClient> ProductsClients { get; set; } = new HashSet<ProductClient>();
}