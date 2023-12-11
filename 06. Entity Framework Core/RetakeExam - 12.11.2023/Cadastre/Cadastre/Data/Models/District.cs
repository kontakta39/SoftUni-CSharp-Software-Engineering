using Cadastre.Data.Enumerations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace Cadastre.Data.Models;

public class District
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
    public string PostalCode { get; set; }

    [Required]
    public Region Region { get; set; }

    public ICollection<Property> Properties { get; set; } = new List<Property>();
}