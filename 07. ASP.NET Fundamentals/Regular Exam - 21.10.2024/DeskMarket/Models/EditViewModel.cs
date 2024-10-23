using DeskMarket.Data.Models;
using System.ComponentModel.DataAnnotations;
using static DeskMarket.Constants.ModelConstants;

namespace DeskMarket.Models;

public class EditViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(ProductNameMaxLength, MinimumLength = ProductNameMinLength)]
    public string ProductName { get; set; } = null!;

    [Required]
    [Range(MinPrice, MaxPrice)]
    public decimal Price { get; set; }

    [Required]
    [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
    public string Description { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
    public string AddedOn { get; set; } = null!;

    [Required]
    public string SellerId { get; set; } = null!;

    [Required]
    public int CategoryId { get; set; }

    public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
}