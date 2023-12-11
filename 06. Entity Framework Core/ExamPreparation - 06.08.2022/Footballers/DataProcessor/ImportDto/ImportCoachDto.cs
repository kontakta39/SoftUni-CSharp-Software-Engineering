using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Coach")]
public class ImportCoachDto
{
    [XmlElement("Name")]
    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Name { get; set; }

    [XmlElement("Nationality")]
    [Required]
    public string Nationality { get; set; }

    [XmlArray("Footballers")]
    public FootballerDto[] Footballers { get; set; }
}

[XmlType("Footballer")]
public class FootballerDto
{
    [XmlElement("Name")]
    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Name { get; set; }

    [XmlElement("ContractStartDate")]
    [Required]
    public string ContractStartDate { get; set; }

    [XmlElement("ContractEndDate")]
    [Required]
    public string ContractEndDate { get; set; }

    [XmlElement("BestSkillType")]
    [Range(0, 4)]
    [Required]
    public int BestSkillType { get; set; }

    [XmlElement("PositionType")]
    [Range(0, 3)]
    [Required]
    public int PositionType { get; set; }
}