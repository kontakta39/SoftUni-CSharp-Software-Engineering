using Cadastre.Data.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cadastre.DataProcessor.ImportDtos;

[XmlType("District")]
public class ImportDistrictDto
{
    [XmlAttribute("Region")]
    public string Region { get; set; }

    [XmlElement("Name")]
    [Required]
    [StringLength(80, MinimumLength = 2)]
    public string Name { get; set; }

    [XmlElement("PostalCode")]
    [Required]
    [RegularExpression(@"^[A-Z]{2}-\d{5}$")]
    public string PostalCode { get; set; }

    [XmlArray("Properties")]
    public PropertyDto[] Properties { get; set; }
}

[XmlType("Property")]
public class PropertyDto
{
    [XmlElement("PropertyIdentifier")]
    [Required]
    [StringLength(20, MinimumLength = 16)]
    public string PropertyIdentifier { get; set; }

    [XmlElement("Area")]
    [Required]
    [Range(0, int.MaxValue)]
    public int Area { get; set; }

    [XmlElement("Details")]
    [StringLength(500, MinimumLength = 5)]
    public string Details { get; set; }

    [XmlElement("Address")]
    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Address { get; set; }

    [XmlElement("DateOfAcquisition")]
    [Required]
    public string DateOfAcquisition { get; set; }
}