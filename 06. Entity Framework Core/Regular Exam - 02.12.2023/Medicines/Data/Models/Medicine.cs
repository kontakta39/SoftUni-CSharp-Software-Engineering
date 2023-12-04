using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models;

public class Medicine
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    [Range(0.01, 1000.00)]
    public decimal Price { get; set; }

    [Required]
    public Category Category { get; set; }

    [Required]
    public DateTime ProductionDate { get; set; }

    [Required]
    public DateTime ExpiryDate { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Producer { get; set; }

    [Required]
    public int PharmacyId { get; set; }

    [ForeignKey(nameof(PharmacyId))]
    public Pharmacy Pharmacy { get; set; }

    public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new List<PatientMedicine>();
}
