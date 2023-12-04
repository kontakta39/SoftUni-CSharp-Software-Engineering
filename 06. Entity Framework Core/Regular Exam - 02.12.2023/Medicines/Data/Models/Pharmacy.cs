using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Medicines.Data.Models;

public class Pharmacy
{
    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [StringLength(14)]
    [RegularExpression(@"\(\d{3}\) \d{3}-\d{4}")]
    public string PhoneNumber { get; set; }

    [JsonIgnore]
    [Required]
    public bool IsNonStop { get; set; }

    [JsonIgnore]
    public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
}
