using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ImportDtos;

[XmlType("Pharmacy")]
public class ImportPharmacyDto
{
    [XmlAttribute("non-stop")]
    [Required]
    public string IsNonStop { get; set; }

    [XmlElement("Name")]
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    [XmlElement("PhoneNumber")]
    [Required]
    [StringLength(14)]
    [RegularExpression(@"\(\d{3}\) \d{3}-\d{4}")]
    public string PhoneNumber { get; set; }

    [XmlArray("Medicines")]
    public MedicinesDto[] Medicines { get; set; }
}

[XmlType("Medicine")]
public class MedicinesDto
{
    [XmlAttribute("category")]
    [Required]
    public string Category { get; set; }

    [XmlElement("Name")]
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Name { get; set; }

    [XmlElement("Price")]
    [Required]
    [Range(0.01, 1000.00)]
    public decimal Price { get; set; }

    [XmlElement("ProductionDate")]
    [Required]
    public string ProductionDate { get; set; }

    [XmlElement("ExpiryDate")]
    [Required]
    public string ExpiryDate { get; set; }

    [XmlElement("Producer")]
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Producer { get; set; }
}