using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models;

public class Property
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 16)]
    public string PropertyIdentifier { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Area { get; set; }

    [StringLength(500, MinimumLength = 5)]
    public string Details { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Address { get; set; }

    [Required]
    public DateTime DateOfAcquisition { get; set; }

    [Required]
    public int DistrictId { get; set; }

    [ForeignKey(nameof(DistrictId))]
    public District District { get; set; }

    public ICollection<PropertyCitizen> PropertiesCitizens { get; set; } = new List<PropertyCitizen>();
}
