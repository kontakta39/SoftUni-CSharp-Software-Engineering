using System.ComponentModel.DataAnnotations;
using static DeskMarket.Constants.ModelConstants;

namespace DeskMarket.Data.Models;

public class Category
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(CategoryNameMaxLength, MinimumLength = CategoryNameMinLength)]
    public string Name { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}