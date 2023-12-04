using Medicines.Data.Models.Enums;
using Medicines.DataProcessor.ImportDtos;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos;

[XmlType("Patient")]
public class ExportPatientDto
{
    [XmlAttribute("Gender")]
    public string Gender { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("AgeGroup")]
    public AgeGroup AgeGroup { get; set; }

    [XmlArray("Medicines")]
    public MedicineDto[] Medicines { get; set; }
}

[XmlType("Medicine")]
public class MedicineDto
{
    [XmlAttribute("Category")]
    public string Category { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; }

    [XmlElement("Price")]
    public string Price { get; set; }

    [XmlElement("Producer")]
    public string Producer { get; set; }

    [XmlElement("BestBefore")]
    public string BestBefore { get; set; }
}